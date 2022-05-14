using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.Data.Models
{
    public class Vehicle
    {
        public long Id { get; set; }
        [Required] public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Typ pojazdu")][Required] public VehicleType VehicleType { get; set; }
        [Display(Name = "Opis")][Required] public string Description { get; set; }
        [Display(Name = "Rocznik")] public string Year { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

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
