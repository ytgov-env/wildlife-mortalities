using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum Sex
{
    [Display(Name = "Female")]
    Female = 10,

    [Display(Name = "Male")]
    Male = 20,

    [Display(Name = "Unknown")]
    Unknown = 30
}
