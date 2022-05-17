namespace abcclicktrans.Data.Models
{
    public class TransportAddress
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Postal { get; set; }
        public string Country { get; set; }

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
