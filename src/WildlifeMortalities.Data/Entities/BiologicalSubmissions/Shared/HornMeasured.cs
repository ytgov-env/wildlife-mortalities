using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

public enum HornMeasured
{
    [Display(Name = "Left horn")]
    LeftHorn = 10,

    [Display(Name = "Right horn")]
    RightHorn = 20
}
