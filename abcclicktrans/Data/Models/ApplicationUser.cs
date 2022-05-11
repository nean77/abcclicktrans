using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace abcclicktrans.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public string CompanyName { get; set; }
        [MaxLength(10)] public string Nip { get; set; }
        public bool IsActive { get; set; }
        public bool IsConfirmed { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Postal { get; set; }
        public string? Country { get; set; }
        public AccountType AccountType { get; set; }
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
