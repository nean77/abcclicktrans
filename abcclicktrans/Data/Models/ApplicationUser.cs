using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace abcclicktrans.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Imię")][Required] public string FirstName { get; set; }
        [Display(Name = "Nazwisko")][Required] public string LastName { get; set; }
        [Display(Name = "Nazwa firmy")] public string CompanyName { get; set; }
        [MaxLength(10)] public string Nip { get; set; }
        [Display(Name = "Konto aktywne")] public bool IsActive { get; set; }
        [Display(Name = "Potwierdzenie dokumentów")] public bool IsConfirmed { get; set; }
        [Display(Name = "Ulica")] public string? Street { get; set; }
        [Display(Name = "Miasto")] public string? City { get; set; }
        [Display(Name = "Kod pocztowy")] public string? Postal { get; set; }
        [Display(Name = "Kraj")] public string? Country { get; set; }
        [Display(Name = "Typ konta")] public AccountType AccountType { get; set; }
        public Guid? SubscriptionIdGuid { get; set; }
        public virtual Subscription? Subscription { get; set; }

        public ApplicationUser(string firstName, string lastName, string email, string passwordHash) :base()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
        }

        public ApplicationUser(string firstName, string lastName, string companyName, string nip, string? street, string? city, string? postal,
            string? country, bool isActive, bool isConfirmed, AccountType accountType) :base()
        {
            FirstName = firstName;
            LastName = lastName;
            CompanyName = companyName;
            Nip = nip;
            Street = street;
            City = city;
            Postal = postal;
            Country = country;
            IsActive = isActive;
            IsConfirmed = isConfirmed;
            AccountType = accountType;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} - {UserName}";
        }
    }
}
