using abcclicktrans.Data;
using abcclicktrans.Data.Models;
using abcclicktrans.Extensions;
using abcclicktrans.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.Security.Claims;

namespace abcclicktrans.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly ILogger<MyAccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _ctx;
        private readonly IMapper _mapper;

        public MyAccountController(SignInManager<ApplicationUser> signInManager, ILogger<MyAccountController> logger,
            IHttpContextAccessor httpContextAccessor, AppDbContext ctx, IMapper mapper)
        {
            _logger = logger;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Vehicles()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var vehicles = await _ctx.Vehicles
                .Where(x => x.ApplicationUserId == userId).ToListAsync();

            foreach (var vehicle in vehicles)
            {
                var x = (VehicleType)Enum.Parse(typeof(VehicleType), vehicle.VehicleType.ToString());
                vehicle.VehicleType = x;
            }

            return View(vehicles);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddVehicle()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddVehicle(Vehicle vehicle)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            vehicle.ApplicationUserId = userId;
            vehicle.TimeStamp = DateTime.Now;
            vehicle.Name = vehicle.VehicleType.GetDisplayName();

            try
            {
                _ctx.Vehicles.Add(vehicle);
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                return View(vehicle);
            }

            return RedirectToAction("Vehicles");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditVehicle(long id)
        {
            var vehicle = await _ctx.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
            return View(vehicle);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditVehicle(Vehicle vehicle)
        {
            try
            {
                vehicle.ModifiedDateTime = DateTime.Now;
                vehicle.Name = vehicle.VehicleType.GetDisplayName();

                _ctx.Vehicles.Update(vehicle);
                await _ctx.SaveChangesAsync();

                return RedirectToAction("Vehicles");
            }
            catch (Exception ex)
            {
                return View(vehicle);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _ctx.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
            if (vehicle.Id == 0)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var vehicle = await _ctx.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
                _ctx.Vehicles.Remove(vehicle);
                await _ctx.SaveChangesAsync();

                return RedirectToAction("Vehicles");
            }
            catch (Exception e)
            {
                throw new Exception("Vehicle can't be deleted");
            }

        }

        [Authorize]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Administration()
        {
            var list = await _ctx.ApplicationUsers
                .Include(x => x.Subscription)
                .Where(x => x.AccountType != AccountType.Admin)
                .ToListAsync();
            var users = new List<UserViewModel>();
            foreach (var userDTO in list)
            {
                if (userDTO.AccountType == AccountType.Supplier)
                {
                    userDTO.Subscription.ExpirationDateTime = userDTO.Subscription.ExpirationDateTime.Date;
                }
                users.Add(_mapper.Map<UserViewModel>(userDTO));
            }

            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> EditSupplier(string id)
        {
            var userDTO = await _ctx.ApplicationUsers
                .Include(x => x.Subscription)
                .FirstOrDefaultAsync(x => x.Id == id);

            var user = _mapper.Map<UserViewModel>(userDTO);

            return View("Edit", user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSupplier(UserViewModel user)
        {
            try
            {
                var userDTO = await _ctx.ApplicationUsers
                    .Include(x => x.Subscription)
                    .FirstOrDefaultAsync(x => x.Id == user.Id);

                userDTO.Subscription.ExpirationDateTime = user.SubscriptionDateTime;
                userDTO.IsActive = user.IsActive;
                userDTO.IsConfirmed = user.IsConfirmed;

                _ctx.ApplicationUsers.Update(userDTO);
                await _ctx.SaveChangesAsync();
                return RedirectToAction("Administration");
            }
            catch
            {
                _logger.LogCritical("Unable to update user from administration panel");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
