using Microsoft.AspNetCore.Identity;

namespace abcclicktrans.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string NIP { get; set; }
        public Address? Address { get; set; }
        public bool IsActive { get; set; }
        public bool IsConfirmed { get; set; }
        public AccountType AccountType { get; set; }
        public Subscription? Subscription { get; set; }

        public ApplicationUser(string firstName, string lastName, string companyName, string nip, Address address, bool isActive, bool isConfirmed) :base()
        {
            FirstName = firstName;
            LastName = lastName;
            CompanyName = companyName;
            NIP = nip;
            Address = address;
            IsActive = isActive;
            IsConfirmed = isConfirmed;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} - {UserName}";
        }
    }

    public enum AccountType
    {
        Customer,
        Supplier,
        Admin,
        SuperAdmin
    }
}
