﻿using System.Security.Claims;
using abcclicktrans.Data;
using abcclicktrans.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            var vehicles = await _ctx.Vehicles.Where(x => x.ApplicationUserId == userId).ToListAsync();

            return View(vehicles);
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
