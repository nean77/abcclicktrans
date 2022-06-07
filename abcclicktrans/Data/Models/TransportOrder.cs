using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.Data.Models
{
    public class TransportOrder
    {
        [Display(Name = "Id zlecenia")] public long Id { get; set; }
        [Display(Name = "Tytuł")][Required] public string Name { get; set; }
        [Display(Name = "Opis transportu")][Required] public string Description { get; set; }
        [Display(Name = "Szerokość")] public string? Width { get; set; }
        [Display(Name = "Wysokość")] public string? Height { get; set; }
        [Display(Name = "Długość")] public string? Length { get; set; }
        [Display(Name = "Waga")] public string? Weight { get; set; }
        [Display(Name = "Wymagana winda")] public bool TailLift { get; set; }
        [Display(Name = "Wymagana pomoc załadunku")] public bool LoadAssistance { get; set; }
        public bool OCP { get; set; }
        public bool ADR { get; set; }
        [Display(Name = "Data utworzenia")] public DateTime CreateDateTime { get; set; }
        [Display(Name = "Adres nadania")] public TransportAddress? PickUpAddress { get; set; }
        [Display(Name = "Data nadania")] public DateTime? PickupDateTime { get; set; }
        [Display(Name = "Adres Dostawy")] public TransportAddress? DeliveryAddress { get; set; }
        [Display(Name = "Data dostawy")] public DateTime? DeliveryDateTime { get; set; }
        [Display(Name = "Dystans")] public string? Distance { get; set; }
        [Display(Name = "Kategoria towaru")][Required] public ParcelCategory Category { get; set; }
        [Display(Name = "Budżet")][DataType(DataType.Currency)] public decimal MaxValue { get; set; }
        [Display(Name = "Obraz")] public string? Image { get; set; }
        [Display(Name = "Id użytkownika")] public string ApplicationUserId { get; set; }
        public string? IPaddress { get; set; }
        public ApplicationUser User { get; set; }
    }
    
}
