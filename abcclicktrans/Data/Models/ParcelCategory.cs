using System.Runtime.Serialization;

namespace abcclicktrans.Data.Models;

public enum ParcelCategory
{
    [EnumMember(Value = "Meble")]
    Furnitures,
    [EnumMember(Value = "Samochody")]
    Cars,
    [EnumMember(Value = "Motocykle, skutery, rowery")]
    SingleTracks,
    [EnumMember(Value = "Palety")]
    Pallets,
    [EnumMember(Value = "Paczki")]
    Parcels,
    [EnumMember(Value = "Zwierzęta")]
    Pets,
    [EnumMember(Value = "Maszyny")]
    Machines,
    [EnumMember(Value = "Inne")]
    Others
}