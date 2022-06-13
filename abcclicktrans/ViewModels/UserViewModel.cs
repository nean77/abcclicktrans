using abcclicktrans.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "Id")][Required] public string Id { get; set; }
        [Display(Name = "Imię")][Required] public string FirstName { get; set; }
        [Display(Name = "Nazwisko")][Required] public string LastName { get; set; }
        [Display(Name = "Email")] public string Email { get; set; }
        [Display(Name = "Email")] public string EmailAddress { get; set; }
        [Display(Name = "Nazwa firmy")] public string? CompanyName { get; set; }
        [MaxLength(10)] public string Nip { get; set; }
        [Display(Name = "Konto aktywne")] public bool IsActive { get; set; }
        [Display(Name = "Potwierdzenie dokumentów")] public bool IsConfirmed { get; set; }
        [Display(Name = "Typ konta")] public AccountType AccountType { get; set; }
        public Guid? SubscriptionIdGuid { get; set; }
        [Display(Name = "Data końca subskrypcji")] public DateTime SubscriptionDateTime { get; set; }
    }
}
