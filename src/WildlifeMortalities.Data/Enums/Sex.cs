using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum Sex
{
    Uninitialized = 0,

    [Display(Name = "Female")]
    Female = 1,

    [Display(Name = "Male")]
    Male = 2,

    [Display(Name = "Unknown")]
    Unknown = 3
}
