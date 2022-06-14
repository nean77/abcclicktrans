using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.Data.Models
{
    public class Parameters
    {
        [Key][Display(Name = "Klucz")] public string Id { get; set; }
        [Required] [MaxLength(255)][Display(Name = "Wartość")] public string Value { get; set; }
        [Display(Name = "Data utworzenia")] public DateTime CreateDateTime { get; set; }
        [Display(Name = "Data modyfikacji")] public DateTime? ModifiedDateTime { get; set; }
        [Display(Name = "Opis")] public string? Description { get; set; }
    }
}
