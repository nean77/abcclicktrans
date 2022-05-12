namespace abcclicktrans.Models
{
    public class MailClass
    {
        public string FromMailId { get; set; } = "abcclicktrans@yahoo.com";
        public string FromMailIdPassword { get; set; } = "cclzvofgehiizxrf";
        public List<string> ToMailIds { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Attachements { get; set; } = new List<string>();
    }
}
