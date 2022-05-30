using abcclicktrans.Data;
using abcclicktrans.Data.Models;
using abcclicktrans.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Claims;

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
        public async Task<IActionResult> Index()
        {
            List<TransportOrderViewModel> orders = new List<TransportOrderViewModel>();
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var ordersDto = await _ctx.TransportOrders
                .Include(u => u.User)
                .Include(pua => pua.PickUpAddress)
                .Include(dlv => dlv.DeliveryAddress)
                .OrderByDescending(x=>x.CreateDateTime)
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
                order.ImageSrc = item.Image;

                bool activeSubscription = false;
                if (userId != null)
                {
                    activeSubscription =
                        _ctx.Subscriptions.FirstOrDefault(x => x.ApplicationUserId == userId).ExpirationDateTime >=
                        DateTime.Now
                            ? true
                            : false;
                }

                if (!User.IsInRole(AccountType.Supplier.ToString()) ||
                    !User.IsInRole(AccountType.Admin.ToString()) ||
                    !activeSubscription)
                {
                    var mail = new MailAddress(item.User.Email);
                    order.User.Email = item.User.Email.Substring(0, 4) + "***@" + mail.Host;
                    if (order.User.PhoneNumber != null)
                        order.User.PhoneNumber = item.User.PhoneNumber.Substring(0, 5) + "****";

                    order.PickUpAddress.Street = "***";
                    order.PickUpAddress.Country = "***";

                    order.DeliveryAddress.Street = "***";
                    order.DeliveryAddress.Country = "***";
                }
                orders.Add(order);
            }

            return View(orders);
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
                order.ImageSrc = item.Image;
                orders.Add(order);
            }

            return View(orders);
        }
        [HttpGet]
        [Authorize]
        public IActionResult AddOrder()
        {
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
                    filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\UploadedPhotos"));
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Bład zapisu pliku");
            }

            return "/UploadedPhotos/" + filename;
        }
    }
}
