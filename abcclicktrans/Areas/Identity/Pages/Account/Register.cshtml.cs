// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using abcclicktrans.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using abcclicktrans.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace abcclicktrans.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            AppDbContext ctx)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _context = ctx;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }
        
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage = "To pole jest wymagane")]
            [Display(Name = "Imię")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "To pole jest wymagane")]
            [Display(Name = "Nazwisko")]
            public string LastName { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "To pole jest wymagane")]
            [EmailAddress(ErrorMessage = "To nie jest poprawny adres email.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "To pole jest wymagane")]
            [StringLength(100, ErrorMessage = "{0} musi mieć minimum {2} znaków długości.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź Hasło")]
            [Required(ErrorMessage = "To pole jest wymagane")]
            [Compare("Password", ErrorMessage = "Hasła muszą do siebie pasować.")]
            public string ConfirmPassword { get; set; }

            public bool Supplier { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                await _userStore.SetUserNameAsync(user, Input.FirstName + " " + Input.LastName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                await CreateRole();
                await AddUserToRole(user);
                if(user.AccountType == AccountType.Supplier)
                {
                    await GenerateSubscription(user);
                }

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Potwierdź swoje konto abcClickTrans.eu",
                        $"Witaj {Input.FirstName}<br />aby dokończyć rejestrację Twojego konta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>KLIKNIJ TUTAJ</a>.<br /><br />" +
                        $"<i>Wiadomość wygenerowana automatyczie prosimy na nią nie odpowiadać.</i>");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task GenerateSubscription(ApplicationUser user)
        {
            _context.Subscriptions.Add(new Subscription(user.Id));
            await _context.SaveChangesAsync();
        }

        private async Task AddUserToRole(ApplicationUser user)
        {
            var customerRole = _roleManager.FindByNameAsync(AccountType.Customer.ToString()).Result;
            var supplierRole = _roleManager.FindByNameAsync(AccountType.Supplier.ToString()).Result;

            if (user.AccountType == AccountType.Supplier && supplierRole != null)
            {
                IdentityResult roleresult = await _userManager.AddToRoleAsync(user, supplierRole.Name);
            }
            if (user.AccountType == AccountType.Customer && customerRole != null)
            {
                IdentityResult roleresult = await _userManager.AddToRoleAsync(user, customerRole.Name);
            }
        }

        private async Task CreateRole()
        {
            foreach (var roleName in Enum.GetValues(typeof(AccountType)))
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName.ToString());
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                   var  roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName.ToString()));
                }
            }
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                //var user = Activator.CreateInstance<ApplicationUser>();
                var user = new ApplicationUser(Input.FirstName, Input.LastName, Input.Email, Input.Password);
                if (Input.Supplier) user.AccountType = AccountType.Supplier;
                else user.AccountType = AccountType.Customer;
                return user;
            }
            catch
            {
                throw new InvalidOperationException($"Wystąpił błąd rejestracji - powiadom administratora");
                //throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                //    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                //    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
