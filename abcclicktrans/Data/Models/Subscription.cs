using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.Data.Models;

public class Subscription
{
    [Display(Name = "Nr subskrypcji")] public Guid Id { get; set; }
    [Required] public string ApplicationUserId { get; set; }
    [Display(Name = "Data rejestracji")] public DateTime RegistredDateTime { get; set; }
    [Display(Name = "Data wygaśnięcia")] public DateTime ExpirationDateTime { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }

    public Subscription(){}
    public Subscription(string applicationUserId)
    {
        Id = Guid.NewGuid();
        ApplicationUserId = applicationUserId;
        RegistredDateTime = DateTime.Now;
        ExpirationDateTime = RegistredDateTime.AddDays(30);
    }
}