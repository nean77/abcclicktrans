using System.Web;
using abcclicktrans.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.ViewModels
{
    public class TransportOrderViewModel
    {
        [Display(Name = "Id zlecenia")] public long Id { get; set; }
        [Display(Name = "Tytuł")][Required] public string Name { get; set; }

        [Required] [Display(Name = "Opis transportu")] public string Description { get; set; }

        [Display(Name = "Wymiary")] public ParcelSize ParcelSize { get; set; }
        [Display(Name = "Wymagana winda")] public bool TailLift { get; set; }

        [Display(Name = "Wymagana pomoc załadunku")] public bool LoadAssistance { get; set; }

        public bool OCP { get; set; }
        public bool ADR { get; set; }
        [Display(Name = "Data utworzenia")] public DateTime CreateDateTime { get; set; }

        [Display(Name = "Adres nadania")] public TransportAddress? PickUpAddress { get; set; } 

        [Display(Name = "Data nadania")][DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")] public DateTime? PickupDateTime { get; set; }

        [Display(Name = "Adres Dostawy")] public TransportAddress? DeliveryAddress { get; set; } 
        [Display(Name = "Data dostawy")][DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")] public DateTime? DeliveryDateTime { get; set; }

        [Display(Name = "Dystans")] public string? Distance { get; set; }

        [Required] [Display(Name = "Kategoria towaru")] public ParcelCategory Category { get; set; }

        [Display(Name = "Budżet")] [DataType(DataType.Currency)] public decimal MaxValue { get; set; }
        [Display(Name="Waluta")]public Currency Currency { get; set; }

        [Display(Name = "Obraz")] public IFormFile? Image { get; set; }
        public string? ImageSrc { get; set; }
        public string? IPaddress { get; set; }
        [Display(Name = "Id użytkownika")] public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }


    }

    public class ParcelSize
    {
        [Display(Name = "Szerokość")] public string? Width { get; set; }
        [Display(Name = "Wysokość")] public string? Height { get; set; }
        [Display(Name = "Długość")] public string? Length { get; set; }
        [Display(Name = "Waga")] public string? Weight { get; set; }
    }
}