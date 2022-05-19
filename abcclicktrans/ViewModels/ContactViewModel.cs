using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.ViewModels
{
    public class ContactViewModel
    {
        [EmailAddress]public string Email { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
