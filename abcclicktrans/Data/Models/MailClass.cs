namespace abcclicktrans.Models
{
    public class MailClass
    {
        public string FromMailId { get; set; } = "notification@abcclicktrans.eu";//"abcclicktrans@yahoo.com";
        public string FromMailIdPassword { get; set; } = "notificationABCgrzybowa2022!";//"cclzvofgehiizxrf";
        public List<string> ToMailIds { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Attachements { get; set; } = new List<string>();
    }
}
