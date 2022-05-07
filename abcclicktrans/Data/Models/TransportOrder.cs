using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.Data.Models
{
    public class TransportOrder
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ParcelSize Size { get; set; }
        public bool TailLift { get; set; }
        public bool LoadAssistance { get; set; }
        public bool OCP { get; set; }
        public bool ADR { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Address PickUpAddress { get; set; }
        public DateTime PickupDateTime { get; set; }
        public Address DeliveryAddress { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public string Distance { get; set; }
        public ParcelCategory Category { get; set; }
        [DataType(DataType.Currency)] public decimal MaxValue { get; set; }
    }

    public class ParcelSize
    {
        public string Width { get; set; }
        public string Height { get; set; }
        public string Length { get; set; }
        public string Weight { get; set; }
    }
}
