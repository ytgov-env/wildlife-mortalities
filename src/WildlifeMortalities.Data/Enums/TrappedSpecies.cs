using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum TrappedSpecies
{
    Uninitialized = AllSpecies.Uninitialized,

    [Display(Name = "Grey wolf")] GreyWolf = AllSpecies.GreyWolf,

    [Display(Name = "Wolverine")] Wolverine = AllSpecies.Wolverine
}
