using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.Data.Models
{
    public enum Currency
    {
        [Display(Name = "EUR")]
        Euro,
        [Display(Name = "PLN")]
        PLN,
        [Display(Name = "DOL")]
        Dollar
    }
}