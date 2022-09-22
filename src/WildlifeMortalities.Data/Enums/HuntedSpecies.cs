using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum HuntedSpecies
{
    Uninitialized = AllSpecies.Uninitialized,

    [Display(Name = "American black bear")]
    AmericanBlackBear = AllSpecies.AmericanBlackBear,

    [Display(Name = "Barren-ground caribou")]
    BarrenGroundCaribou = AllSpecies.BarrenGroundCaribou,

    [Display(Name = "Coyote")]
    Coyote = AllSpecies.Coyote,

    [Display(Name = "Elk")]
    Elk = AllSpecies.Elk,

    [Display(Name = "Grey wolf")]
    GreyWolf = AllSpecies.GreyWolf,

    [Display(Name = "Grizzly bear")]
    GrizzlyBear = AllSpecies.GrizzlyBear,

    [Display(Name = "Moose")]
    Moose = AllSpecies.Moose,

    [Display(Name = "Mountain goat")]
    MountainGoat = AllSpecies.MountainGoat,

    [Display(Name = "Mule deer")]
    MuleDeer = AllSpecies.MuleDeer,

    [Display(Name = "Thinhorn sheep")]
    ThinhornSheep = AllSpecies.ThinhornSheep,

    [Display(Name = "White-tailed deer")]
    WhiteTailedDeer = AllSpecies.WhiteTailedDeer,

    [Display(Name = "Wolverine")]
    Wolverine = AllSpecies.Wolverine,

    [Display(Name = "Wood bison")]
    WoodBison = AllSpecies.WoodBison,

    [Display(Name = "Woodland caribou")]
    WoodlandCaribou = AllSpecies.WoodlandCaribou
}
