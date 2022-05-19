using abcclicktrans.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using abcclicktrans.Data.Models;
using abcclicktrans.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace abcclicktrans.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public HomeController(UserManager<ApplicationUser> userManager, ILogger<HomeController> logger,
            IEmailSender emailSender)
        {
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["confirmed"] = "true";
            if (user != null && user.AccountType == AccountType.Supplier && !user.IsConfirmed)
            {
                ViewData["confirmed"] = "false";
                ViewData["Message"] = "Twoje konto nie zostało jeszcze potwierdzone, prześlij wymagane dokumenty !";
                ViewData["Alert"] = "warning";
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel cvm)
        {
            try
            {
                var message = cvm.Message + "<br/><br/><b> Wysłane ze strony przez: </b>" + cvm.Email;
                await _emailSender.SendEmailAsync("abcclicktrans@yahoo.com", cvm.Subject, message);
            }
            catch
            {
                return View(cvm);
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}