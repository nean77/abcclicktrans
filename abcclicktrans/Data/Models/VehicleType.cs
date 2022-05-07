using System.Runtime.Serialization;

namespace abcclicktrans.Data.Models;

public enum VehicleType 
{
    [EnumMember(Value = "Samochód < 3,5t")]
    Car,
    [EnumMember(Value = "Bus")]
    Van,
    [EnumMember(Value = "Laweta")]
    TowTruck,
    [EnumMember(Value = "Ciężarowy > 3,5t")]
    Lorry

}