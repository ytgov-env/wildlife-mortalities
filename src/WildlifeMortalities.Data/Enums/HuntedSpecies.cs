using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum HuntedSpecies
{
    Uninitialized = AllSpecies.Uninitialized,
    [Display(Name = "American Black Bear")]
    AmericanBlackBear = AllSpecies.AmericanBlackBear,
    [Display(Name = "Barren Ground Caribou")]
    BarrenGroundCaribou = AllSpecies.BarrenGroundCaribou,
    [Display(Name = "Coyote")]
    Coyote = AllSpecies.Coyote,
    [Display(Name = "Elk")]
    Elk = AllSpecies.Elk,
    [Display(Name = "Grey Wolf")]
    GreyWolf = AllSpecies.GreyWolf,
    [Display(Name = "Grizzly Bear")]
    GrizzlyBear = AllSpecies.GrizzlyBear,
    [Display(Name = "Moose")]
    Moose = AllSpecies.Moose,
    [Display(Name = "Mountain Goat")]
    MountainGoat = AllSpecies.MountainGoat,
    [Display(Name = "Mule Deer")]
    MuleDeer = AllSpecies.MuleDeer,
    [Display(Name = "Thinhorn Sheep")]
    ThinhornSheep = AllSpecies.ThinhornSheep,
    [Display(Name = "White-tailed Deer")]
    WhiteTailedDeer = AllSpecies.WhiteTailedDeer,
    [Display(Name = "Wolverine")]
    Wolverine = AllSpecies.Wolverine,
    [Display(Name = "Wood Bison")]
    WoodBison = AllSpecies.WoodBison,
    [Display(Name = "Woodland Caribou")]
    WoodlandCaribou = AllSpecies.WoodlandCaribou
}
