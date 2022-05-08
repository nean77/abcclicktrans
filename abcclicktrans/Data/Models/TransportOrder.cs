using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.Data.Models
{
    public class TransportOrder
    {
        public long Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        public string? Width { get; set; }
        public string? Height { get; set; }
        public string? Length { get; set; }
        public string? Weight { get; set; }
        public bool TailLift { get; set; }
        public bool LoadAssistance { get; set; }
        public bool OCP { get; set; }
        public bool ADR { get; set; }
        public DateTime CreateDateTime { get; set; }
        [Required] public TransportAddress PickUpAddress { get; set; }
        public DateTime? PickupDateTime { get; set; }
        [Required] public TransportAddress DeliveryAddress { get; set; }
        public DateTime? DeliveryDateTime { get; set; }
        public string? Distance { get; set; }
        [Required] public ParcelCategory Category { get; set; }
        [DataType(DataType.Currency)] public decimal MaxValue { get; set; }

        /*public TransportOrder(string name, string description, ParcelSize size, bool tailLift, bool loadAssistance,
            bool ocp, bool adr, TransportAddress pickUpAddress, TransportAddress deliveryAddress, ParcelCategory category, decimal maxValue)
        {
            Name = name;
            Description = description;
            Size = size;
            TailLift = tailLift;
            LoadAssistance = loadAssistance;
            OCP = ocp;
            ADR = adr;
            DeliveryAddress = deliveryAddress;
            Category = category;
            MaxValue = maxValue;
            PickUpAddress = pickUpAddress;
            CreateDateTime = DateTime.Now;
        }
        public TransportOrder(string name, string description, ParcelSize size, bool tailLift, bool loadAssistance,
            bool ocp, bool adr, TransportAddress pickUpAddress, DateTime pickupDateTime, TransportAddress deliveryAddress, 
            DateTime deliveryDateTime, ParcelCategory category, decimal maxValue) : this(name, description, size, tailLift, loadAssistance,
                ocp, adr, pickUpAddress, deliveryAddress, category, maxValue)
        {
            PickupDateTime = pickupDateTime;
            DeliveryDateTime = deliveryDateTime;
        }
        */
    }

    public class ParcelSize
    {
        public string? Width { get; set; }
        public string? Height { get; set; }
        public string? Length { get; set; }
        public string? Weight { get; set; }
    }
}
