using System.Net;
using abcclicktrans.Data;
using abcclicktrans.Data.Models;
using abcclicktrans.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Reflection;
using System.Security.Claims;
using abcclicktrans.Exceptions;
using abcclicktrans.Services;
using Microsoft.AspNetCore.Http.Features;

namespace abcclicktrans.Controllers
{
    public class TransportOrderController : Controller
    {
        private readonly ILogger<MyAccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _ctx;
        private readonly IMapper _mapper;

        public TransportOrderController(SignInManager<ApplicationUser> signInManager, ILogger<MyAccountController> logger,
            IHttpContextAccessor httpContextAccessor, AppDbContext ctx, IMapper mapper)
        {
            _logger = logger;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber=1)
        {
            List<TransportOrderViewModel> orders = new List<TransportOrderViewModel>();

            var ordersDto = await _ctx.TransportOrders
                .Include(u => u.User)
                .Include(pua => pua.PickUpAddress)
                .Include(dlv => dlv.DeliveryAddress)
                .OrderByDescending(x=>x.CreateDateTime)
                .ToListAsync();

            try
            {
                orders = await ConvertOrderDTOToOrders(ordersDto);
            }
            catch (UserLogedOutException ex)
            {
                return RedirectToAction("Index", "Home");
            }
            

            return View(await PaginatedList<TransportOrderViewModel>.CreateAsync(orders, pageNumber, 15));
        }

        [HttpPost]
        public async Task<IActionResult> FilteredOrders(string searchString, bool notUsed, int pageNumber = 1)
        {
            List<TransportOrderViewModel> orders = new List<TransportOrderViewModel>();
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var ordersDto = await _ctx.TransportOrders
                .Include(u => u.User)
                .Include(pua => pua.PickUpAddress)
                .Include(dlv => dlv.DeliveryAddress)
                .OrderByDescending(x => x.CreateDateTime)
                .Where(x=>(x.DeliveryAddress.City.Contains(searchString)) || (x.PickUpAddress.City.Contains(searchString)))
                .ToListAsync();

            try
            {
                orders = await ConvertOrderDTOToOrders(ordersDto);
            }
            catch (UserLogedOutException ex)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("Index", await PaginatedList<TransportOrderViewModel>.CreateAsync(orders, pageNumber, 15));
            //return View(orders);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            List<TransportOrderViewModel> orders = new List<TransportOrderViewModel>();
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var ordersDto = await _ctx.TransportOrders
                .Include(u => u.User)
                .Include(pua => pua.PickUpAddress)
                .Include(dlv => dlv.DeliveryAddress)
                .Where(x => x.ApplicationUserId == userId)
                .ToListAsync();

            foreach (var item in ordersDto)
            {
                var order = _mapper.Map<TransportOrderViewModel>(item);
                order.ParcelSize = new ParcelSize
                {
                    Height = item.Height,
                    Length = item.Length,
                    Weight = item.Weight,
                    Width = item.Width
                };
                order.ImageSrc = "/" + item.Image;
                orders.Add(order);
            }

            return View(orders);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddOrder()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _ctx.ApplicationUsers.FirstOrDefault(x => x.Id == userId);
            if (user != null && user.IsActive == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrder(TransportOrderViewModel order)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            order.ApplicationUserId = userId;
            try
            {
                IPAddress remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                if (remoteIpAddress != null)
                {
                    // If we got an IPV6 address, then we need to ask the network for the IPV4 address 
                    // This usually only happens when the browser is on the same machine as the server.
                    if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
                            .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    }
                    order.IPaddress = remoteIpAddress.ToString();
                }
                string uniqueFile = await UploadedFile(order.Image);
                var orderDto = _mapper.Map<TransportOrder>(order);
                orderDto.Image = uniqueFile;
                orderDto.CreateDateTime = DateTime.Now;
                _ctx.TransportOrders.Add(orderDto);
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                ViewData["FromAdd"] = "true";
                ViewData["Message"] = "Błąd podczas dodawania ogłoszenia !";
                ViewData["Alert"] = "danger";
                return View(order);
            }
            ViewData["FromAdd"] = "true";
            ViewData["Message"] = "Twoje ogłoszenie zostało dodane do bazy !";
            ViewData["Alert"] = "success";
            return RedirectToAction("Index", "TransportOrder");
        }

        private async Task<string> UploadedFile(IFormFile file)
        {
            if (file == null || file.Length == 0) return "";
            string path = "";
            string filename = "";
            try
            {
                if (file != null && file.Length > 0)
                {
                    filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\wwwroot\\assets\\img\\UploadedPhotos";
                    //return path;
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("Bład zapisu pliku");
            }

            return "/assets/img/UploadedPhotos/" + filename;
        }

        private async Task<List<TransportOrderViewModel>> ConvertOrderDTOToOrders(List<TransportOrder> ordersDto)
        {
            List<TransportOrderViewModel> orders = new List<TransportOrderViewModel>();
            foreach (var item in ordersDto)
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var order = _mapper.Map<TransportOrderViewModel>(item);
                order.ParcelSize = new ParcelSize
                {
                    Height = item.Height,
                    Length = item.Length,
                    Weight = item.Weight,
                    Width = item.Width
                };
                order.ImageSrc = item.Image;

                bool activeSubscription = false;
                bool docConfirmed = false;
                if (userId != null)
                {
                    var user = _ctx.ApplicationUsers.FirstOrDefault(x => x.Id == userId);
                    if (user != null && user.IsActive == false)
                    {
                        await _signInManager.SignOutAsync();
                        throw new UserLogedOutException();
                        //return RedirectToAction("Index", "Home");
                    }

                    var sub = _ctx.Subscriptions.FirstOrDefault(x => x.ApplicationUserId == userId);
                    docConfirmed = user.IsConfirmed;
                    if (sub != null)
                        activeSubscription = sub.ExpirationDateTime >= DateTime.Now ?
                            true : false;
                    else activeSubscription = false;
                }

                if (User.IsInRole(AccountType.Admin.ToString()) ||
                    (User.IsInRole(AccountType.Supplier.ToString()) && activeSubscription && docConfirmed))
                {
                    orders.Add(order);
                    continue;
                }

                var mail = new MailAddress(item.User.Email);
                try
                {
                    order.User.Email = item.User.Email.Substring(0, 3) + "***@" + mail.Host;
                }
                catch
                {
                    order.User.Email = "błąd";
                }
                
                if (order.User.PhoneNumber != null)
                {
                    try
                    {
                        order.User.PhoneNumber = item.User.PhoneNumber.Substring(0, 4) + "****";
                    }
                    catch
                    {
                        order.User.PhoneNumber = "brak";
                    }
                }
                order.PickUpAddress.Street = "***";
                order.PickUpAddress.Country = "***";

                order.DeliveryAddress.Street = "***";
                order.DeliveryAddress.Country = "***";

                orders.Add(order);
            }

            return orders;
        }

    }
}
