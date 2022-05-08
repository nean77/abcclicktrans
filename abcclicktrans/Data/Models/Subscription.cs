using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.Data.Models;

public class Subscription
{
    public Guid Id { get; set; }
    [Required] public string ApplicationUserId { get; set; }
    public DateTime RegistredDateTime { get; set; }
    public DateTime ExpirationDateTime { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }

    public Subscription(string applicationUserId)
    {
        Id = Guid.NewGuid();
        ApplicationUserId = applicationUserId;
        RegistredDateTime = DateTime.Now;
        ExpirationDateTime = RegistredDateTime.AddDays(30);
    }
}