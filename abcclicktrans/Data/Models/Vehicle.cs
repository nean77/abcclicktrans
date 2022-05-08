using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.Data.Models
{
    public class Vehicle
    {
        public long Id { get; set; }
        [Required] public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required] public VehicleType VehicleType { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public DateTime TimeStamp { get; set; }

        //public Vehicle(ApplicationUser user, VehicleType type, string description, string year)
        //{
        //    ApplicationUserId = user.Id;
        //    VehicleType = type;
        //    Description = description;
        //    Year = year;
        //    TimeStamp = DateTime.Now;
        //}

        public override string ToString()
        {
            return VehicleType.ToString() + " " + Description + " Rocznik: " + Year;
        }
    }
}
