using System.ComponentModel.DataAnnotations;

namespace abcclicktrans.Data.Models;

public enum AccountType
{
    [Display(Name = "Zleceniodawca")]
    Customer,
    [Display(Name = "Przewoźnik")]
    Supplier,
    Admin,
    SuperAdmin
}