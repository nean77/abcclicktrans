using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.Data.Models
{
    public class TransportAddress
    {
        public Guid Id { get; set; }
        [Display(Name = "Ulica")]
        public string? Street { get; set; }
        [Display(Name = "Miasto")]
        public string? City { get; set; }
        [Display(Name = "Kod pocztowy")]
        public string? Postal { get; set; }
        [Display(Name = "Kraj")]
        public string? Country { get; set; }

        public TransportAddress()
        {
            Id = Guid.NewGuid();
        }

        public TransportAddress(string street, string city, string postal, string country) : this()
        {
            this.Street = street;
            this.City = city;
            this.Postal = postal;
            this.Country = country;
        }
    }

}
