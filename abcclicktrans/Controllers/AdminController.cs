using abcclicktrans.Data;
using abcclicktrans.Data.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.Security.Claims;
using abcclicktrans.Exceptions;
using abcclicktrans.ViewModels;

namespace abcclicktrans.Controllers
{

    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly ILogger<MyAccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _ctx;
        private readonly IMapper _mapper;

        public AdminController(SignInManager<ApplicationUser> signInManager, ILogger<MyAccountController> logger,
            IHttpContextAccessor httpContextAccessor, AppDbContext ctx, IMapper mapper)
        {
            _logger = logger;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _ctx = ctx;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();

            model.Orders = await _ctx.TransportOrders.CountAsync();
            model.Customers =
                await _ctx.ApplicationUsers.Where(x => x.AccountType == AccountType.Customer).CountAsync();
            model.Suppliers = await _ctx.ApplicationUsers.Where(x => x.AccountType == AccountType.Supplier).CountAsync();
            model.ActiveSubscriptions =
                await _ctx.Subscriptions.Where(x => x.ExpirationDateTime >= DateTime.Now).CountAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var list = await _ctx.ApplicationUsers
                .Where(x => x.AccountType == AccountType.Customer)
                .ToListAsync();
            var users = new List<UserViewModel>();

            foreach (var userDTO in list)
            {
                users.Add(_mapper.Map<UserViewModel>(userDTO));
            }

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Suppliers()
        {
            var list = await _ctx.ApplicationUsers
                .Include(x => x.Subscription)
                .Where(x => x.AccountType == AccountType.Supplier)
                .ToListAsync();
            var suppliers = new List<UserViewModel>();

            foreach (var userDTO in list)
            {
                if (userDTO.AccountType == AccountType.Supplier)
                {
                    userDTO.Subscription.ExpirationDateTime = userDTO.Subscription.ExpirationDateTime.Date;
                }
                suppliers.Add(_mapper.Map<UserViewModel>(userDTO));
            }

            return View(suppliers);
        }

        [HttpGet]
        public async Task<IActionResult> Orders(int pageNumber = 1)
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

            return View(await PaginatedList<TransportOrderViewModel>.CreateAsync(orders, pageNumber, 15));
        }

        [HttpGet]
        public IActionResult DeleteOrder(long id)
        {
            var order = _ctx.TransportOrders.FirstOrDefault(x => x.Id == id);
            return View(order);
        }
        [HttpPost, ActionName("DeleteOrder")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteOrderConfirmed(long id)
        {
            try
            {
                var order = _ctx.TransportOrders.FirstOrDefault(x => x.Id == id);
                if (order != null)
                {
                    _ctx.TransportOrders.Remove(order);
                    _ctx.SaveChanges();
                    return RedirectToAction("Orders");
                }
            }
            catch
            {
                _logger.LogCritical("Error during delete order in Admin Mode");
                return View("DeleteOrder");
            }

            return View("Orders");
        }

        [HttpGet]
        public IActionResult EditCustomer(string id)
        {
            try
            {
                var user = _ctx.ApplicationUsers.FirstOrDefault(x => x.Id == id);
                if (user == null) return RedirectToAction("Users");

                var userVM = new UserViewModel();
                userVM = _mapper.Map<UserViewModel>(user);

                return View(userVM);
            }
            catch
            {
                _logger.LogError("Cannot map Customer to VM in Admin Mode");
                return RedirectToAction("Users");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(UserViewModel userVM)
        {
            try
            {
                var user = _ctx.ApplicationUsers.FirstOrDefault(x => x.Id == userVM.Id);
                if (user == null) return RedirectToAction("Users");

                user.IsActive = userVM.IsActive;
                user.IsConfirmed = userVM.IsConfirmed;

                _ctx.ApplicationUsers.Update(user);
                await _ctx.SaveChangesAsync();

                return RedirectToAction("Users");
            }
            catch
            {
                _logger.LogCritical("Error during update user in Admin Mode");
                return RedirectToAction("Users");
            }

        }

        [HttpGet]
        public IActionResult EditSupplier(string id)
        {
            try
            {
                var user = _ctx.ApplicationUsers.FirstOrDefault(x => x.Id == id);
                if (user == null) return RedirectToAction("Suppliers");

                var userVM = new UserViewModel();
                if (user.Subscription != null)
                {
                    user.Subscription.ExpirationDateTime = user.Subscription.ExpirationDateTime.Date;
                }
                userVM = _mapper.Map<UserViewModel>(user);

                return View(userVM);
            }
            catch
            {
                _logger.LogError("Cannot map Supplier to VM in Admin Mode");
                return RedirectToAction("Suppliers");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSupplier(UserViewModel userVM)
        {
            try
            {
                var userDTO = await _ctx.ApplicationUsers
                    .Include(x => x.Subscription)
                    .FirstOrDefaultAsync(x => x.Id == userVM.Id);

                userDTO.Subscription.ExpirationDateTime = userVM.SubscriptionDateTime;
                userDTO.IsActive = userVM.IsActive;
                userDTO.IsConfirmed = userVM.IsConfirmed;
                userDTO.SubscriptionIdGuid = userDTO.Subscription.Id;
                _ctx.ApplicationUsers.Update(userDTO);
                await _ctx.SaveChangesAsync();
                return RedirectToAction("Suppliers");
            }
            catch
            {
                _logger.LogCritical("Error during update supplier in Admin Mode");
                return RedirectToAction("Suppliers");
            }
        }
    }
}
