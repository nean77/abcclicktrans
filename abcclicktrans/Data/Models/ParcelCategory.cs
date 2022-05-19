using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace abcclicktrans.Data.Models;

public enum ParcelCategory
{
    [EnumMember(Value = "Meble")]
    [Display(Name = "Meble")]
    Furnitures,
    [EnumMember(Value = "Samochody")]
    [Display(Name = "Samochody")]
    Cars,
    [EnumMember(Value = "Motocykle, skutery, rowery")]
    [Display(Name = "Motocykle, skutery, rowery")]
    SingleTracks,
    [EnumMember(Value = "Palety")]
    [Display(Name = "Palety")]
    Pallets,
    [EnumMember(Value = "Paczki")]
    [Display(Name = "Paczki")]
    Parcels,
    [EnumMember(Value = "Zwierzęta")]
    [Display(Name = "Zwierzęta")]
    Pets,
    [EnumMember(Value = "Maszyny")]
    [Display(Name = "Maszyny")]
    Machines,
    [EnumMember(Value = "Inne")]
    [Display(Name = "Inne")]
    Others
}