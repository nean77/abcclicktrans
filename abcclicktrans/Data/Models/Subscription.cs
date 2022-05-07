namespace abcclicktrans.Data.Models;

public class Subscription
{
    public Guid Id { get; set; }
    public string ApplicationUserIdGuid { get; set; }
    public DateTime RegistredDateTime { get; set; }
    public DateTime ExpirationDateTime { get; set; }

    public virtual ApplicationUser? ApplicationUser { get; set; }

    public Subscription(string userId)
    {
        Id = Guid.NewGuid();
        ApplicationUserIdGuid = userId;
        RegistredDateTime = DateTime.Now;
        ExpirationDateTime = RegistredDateTime.AddDays(30);
    }
}