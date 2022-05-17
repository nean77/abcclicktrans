using System.ComponentModel.DataAnnotations;
using System.Reflection;
using abcclicktrans.Data;
using abcclicktrans.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using abcclicktrans.Extensions;
using Microsoft.OpenApi.Extensions;

namespace abcclicktrans.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly ILogger<MyAccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _ctx;

        public MyAccountController(SignInManager<ApplicationUser> signInManager, ILogger<MyAccountController> logger,
            IHttpContextAccessor httpContextAccessor, AppDbContext ctx)
        {
            _logger = logger;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _ctx = ctx;
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

    }
}
