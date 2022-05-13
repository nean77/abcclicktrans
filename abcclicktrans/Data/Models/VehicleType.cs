using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace abcclicktrans.Data.Models;

public enum VehicleType 
{
    [EnumMember(Value = "Samochód < 3,5t")]
    [Display(Name = "Samochód < 3,5t")]
    Car,
    [EnumMember(Value = "Bus")]
    [Display(Name = "Bus")]
    Van,
    [EnumMember(Value = "Laweta")]
    [Display(Name = "Laweta")]
    TowTruck,
    [EnumMember(Value = "Ciężarowy > 3,5t")]
    [Display(Name = "Ciężarowy > 3,5t")]
    Lorry

}