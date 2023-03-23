using System.ComponentModel.DataAnnotations;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Enums;

[AttributeUsage(AttributeTargets.Field)]
public class IsOutfitterGuidedHuntableAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Field)]
public class IsSpecialGuidedHuntableAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Field)]
public class IsIndividualHuntableAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Field)]
public class IsTrappableAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Field)]
public class HasGameManagementAreaAttribute : Attribute { }

public enum Species
{
    [IsTrappable]
    [Display(Name = SpeciesConstants.AmericanBeaver)]
    AmericanBeaver = 10,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = SpeciesConstants.AmericanBlackBear)]
    AmericanBlackBear = 20,

    [IsTrappable]
    [Display(Name = SpeciesConstants.AmericanMartin)]
    AmericanMartin = 30,

    [IsTrappable]
    [Display(Name = SpeciesConstants.AmericanMink)]
    AmericanMink = 40,

    [IsTrappable]
    [Display(Name = SpeciesConstants.ArcticFox)]
    ArcticFox = 50,

    [Display(Name = SpeciesConstants.ArcticGroundSquirrel)]
    ArcticGroundSquirrel = 60,

    [Display(Name = SpeciesConstants.BushyTailedWoodrat)]
    BushyTailedWoodrat = 70,

    [IsTrappable]
    [Display(Name = SpeciesConstants.CanadaLynx)]
    CanadaLynx = 80,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = SpeciesConstants.Caribou)]
    Caribou = 90,

    [Display(Name = SpeciesConstants.CollaredPika)]
    CollaredPika = 100,

    [Display(Name = SpeciesConstants.Cougar)]
    Cougar = 110,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [IsTrappable]
    [Display(Name = SpeciesConstants.Coyote)]
    Coyote = 120,

    [IsIndividualHuntable]
    [Display(Name = SpeciesConstants.Elk)]
    Elk = 130,

    [IsTrappable]
    [Display(Name = SpeciesConstants.Ermine)]
    Ermine = 140,

    [IsTrappable]
    [Display(Name = SpeciesConstants.Fisher)]
    Fisher = 150,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [IsTrappable]
    [Display(Name = SpeciesConstants.GreyWolf)]
    GreyWolf = 160,

    [Display(Name = SpeciesConstants.Grouse)]
    Grouse = 161,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = SpeciesConstants.GrizzlyBear)]
    GrizzlyBear = 170,

    [Display(Name = SpeciesConstants.HoaryMarmot)]
    HoaryMarmot = 180,

    [Display(Name = SpeciesConstants.LeastChipmunk)]
    LeastChipmunk = 190,

    [IsTrappable]
    [Display(Name = SpeciesConstants.LeastWeasel)]
    LeastWeasel = 200,

    [Display(Name = SpeciesConstants.Lemming)]
    Lemming = 210,

    [Display(Name = SpeciesConstants.LittleBrownBat)]
    LittleBrownBat = 220,

    [Display(Name = SpeciesConstants.MeadowJumpingMouse)]
    MeadowJumpingMouse = 230,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = SpeciesConstants.Moose)]
    Moose = 240,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = SpeciesConstants.MountainGoat)]
    MountainGoat = 250,

    [IsIndividualHuntable]
    [HasGameManagementArea]
    [Display(Name = SpeciesConstants.MuleDeer)]
    MuleDeer = 260,

    [Display(Name = SpeciesConstants.Muskox)]
    Muskox = 270,

    [IsTrappable]
    [Display(Name = SpeciesConstants.Muskrat)]
    Muskrat = 280,

    [Display(Name = SpeciesConstants.NorthAmericanDeermouse)]
    NorthAmericanDeermouse = 290,

    [Display(Name = SpeciesConstants.NorthAmericanPorcupine)]
    NorthAmericanPorcupine = 300,

    [Display(Name = SpeciesConstants.NorthernFlyingSquirrel)]
    NorthernFlyingSquirrel = 310,

    [Display(Name = SpeciesConstants.NorthernLongEaredBat)]
    NorthernLongEaredBat = 320,

    [IsTrappable]
    [Display(Name = SpeciesConstants.NorthernRiverOtter)]
    NorthernRiverOtter = 330,

    [Display(Name = SpeciesConstants.PolarBear)]
    PolarBear = 340,

    [Display(Name = SpeciesConstants.Ptarmigan)]
    Ptarmigan = 341,

    [IsTrappable]
    [Display(Name = SpeciesConstants.RedFox)]
    RedFox = 350,

    [IsTrappable]
    [Display(Name = SpeciesConstants.RedSquirrel)]
    RedSquirrel = 360,

    [Display(Name = SpeciesConstants.Shrew)]
    Shrew = 370,

    [Display(Name = SpeciesConstants.SnowshoeHare)]
    SnowshoeHare = 380,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = SpeciesConstants.ThinhornSheep)]
    ThinhornSheep = 390,

    [Display(Name = SpeciesConstants.Vole)]
    Vole = 400,

    [IsIndividualHuntable]
    [Display(Name = SpeciesConstants.WhiteTailedDeer)]
    WhiteTailedDeer = 410,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsTrappable]
    [Display(Name = SpeciesConstants.Wolverine)]
    Wolverine = 420,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = SpeciesConstants.WoodBison)]
    WoodBison = 430,

    [Display(Name = SpeciesConstants.Woodchuck)]
    Woodchuck = 440,

    [Display(Name = SpeciesConstants.BirdOther)]
    BirdOther = 450
}
