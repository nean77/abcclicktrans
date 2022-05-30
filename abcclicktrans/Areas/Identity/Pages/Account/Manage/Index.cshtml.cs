// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using abcclicktrans.Data;
using abcclicktrans.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace abcclicktrans.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _ctx;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext ctx)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ctx = ctx;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        [Display(Name = "Typ konta")] public AccountType AccountType { get; set; }

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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Numer telefonu")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Imię")] public string FirstName { get; set; }
            [Display(Name = "Nazwisko")] public string LastName { get; set; }
            [Display(Name = "Nazwa firmy")] public string CompanyName { get; set; }
            [MaxLength(10)][Display(Name = "NIP")] public string NIP { get; set; }
            [Display(Name = "Potwierdzone konto")] public bool IsConfirmed { get; set; }
            [Display(Name = "Ulica")] public string? Street { get; set; }
            [Display(Name = "Miasto")] public string? City { get; set; }
            [Display(Name = "Kod pocztowy")] public string? Postal { get; set; }
            [Display(Name = "Kraj")] public string? Country { get; set; }
            public bool CompanyAccount { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            AccountType = user.AccountType;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CompanyName = user.CompanyName,
                NIP = user.Nip,
                IsConfirmed = user.IsConfirmed,
                Street = user.Street,
                City = user.City,
                Postal = user.Postal,
                Country = user.Country
            };

            if (this.AccountType == AccountType.Supplier) Input.CompanyAccount = true;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie odnaleziono użytkownika '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            if (user.AccountType == AccountType.Supplier)
            {
                var s = await _ctx.Subscriptions.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id);
                ViewData["sub"] = (s.ExpirationDateTime - DateTime.Now).Days;
                ViewData["subNo"] = s.Id.ToString();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można załadować użytkownika o identyfikatorze '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user = MapFromInput(user);
            try
            {
                _ctx.ApplicationUsers.Update(user);
                var res = await _ctx.SaveChangesAsync();

                if (res > 0)
                {
                    StatusMessage = "Profil został zaktualizowany.";
                    return RedirectToPage();
                }
            }
            catch
            {
                StatusMessage = "Nieoczekiwany błąd podczas próby ustawienia aktualizacji.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Twój profil został zaktualizowany.";
            return RedirectToPage();
        }

        private ApplicationUser MapFromInput(ApplicationUser user)
        {
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.CompanyName = Input.CompanyName;
            user.Nip = Input.NIP;
            user.Street = Input.Street;
            user.City = Input.City;
            user.Postal = Input.Postal;
            user.Country = Input.Country;
            user.PhoneNumber = Input.PhoneNumber;

            return user;
        }
    }
}
