using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum TrappedSpecies
{
    Uninitialized = AllSpecies.Uninitialized,
    [Display(Name = "Grey Wolf")]
    GreyWolf = AllSpecies.GreyWolf,
    [Display(Name = "Wolverine")]
    Wolverine = AllSpecies.Wolverine
}
