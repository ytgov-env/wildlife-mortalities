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

[AttributeUsage(AttributeTargets.Field)]
public class IsBigGameAttribute : Attribute { }

public enum Species
{
    [IsTrappable]
    [Display(Name = SpeciesConstants.AmericanBeaver)]
    AmericanBeaver = 100,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [IsBigGame]
    [Display(Name = SpeciesConstants.AmericanBlackBear)]
    AmericanBlackBear = 200,

    [IsTrappable]
    [Display(Name = SpeciesConstants.AmericanMartin)]
    AmericanMartin = 300,

    [IsTrappable]
    [Display(Name = SpeciesConstants.AmericanMink)]
    AmericanMink = 400,

    [IsTrappable]
    [Display(Name = SpeciesConstants.ArcticFox)]
    ArcticFox = 500,

    [Display(Name = SpeciesConstants.ArcticGroundSquirrel)]
    ArcticGroundSquirrel = 600,

    [Display(Name = SpeciesConstants.BushyTailedWoodrat)]
    BushyTailedWoodrat = 700,

    [IsTrappable]
    [Display(Name = SpeciesConstants.CanadaLynx)]
    CanadaLynx = 800,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [IsBigGame]
    [Display(Name = SpeciesConstants.Caribou)]
    Caribou = 900,

    [Display(Name = SpeciesConstants.CollaredPika)]
    CollaredPika = 1000,

    [Display(Name = SpeciesConstants.Cougar)]
    Cougar = 1100,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [IsTrappable]
    [IsBigGame]
    [Display(Name = SpeciesConstants.Coyote)]
    Coyote = 1200,

    [Display(Name = SpeciesConstants.DuskyGrouse)]
    DuskyGrouse = 1201,

    [IsIndividualHuntable]
    [IsBigGame]
    [Display(Name = SpeciesConstants.Elk)]
    Elk = 1300,

    [IsTrappable]
    [Display(Name = SpeciesConstants.Ermine)]
    Ermine = 1400,

    [IsTrappable]
    [Display(Name = SpeciesConstants.Fisher)]
    Fisher = 1500,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [IsTrappable]
    [IsBigGame]
    [Display(Name = SpeciesConstants.GreyWolf)]
    GreyWolf = 1600,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [HasGameManagementArea]
    [IsBigGame]
    [Display(Name = SpeciesConstants.GrizzlyBear)]
    GrizzlyBear = 1700,

    [Display(Name = SpeciesConstants.Grouse)]
    Grouse = 1701,

    [Display(Name = SpeciesConstants.HoaryMarmot)]
    HoaryMarmot = 1800,

    [Display(Name = SpeciesConstants.LeastChipmunk)]
    LeastChipmunk = 1900,

    [IsTrappable]
    [Display(Name = SpeciesConstants.LeastWeasel)]
    LeastWeasel = 2000,

    [Display(Name = SpeciesConstants.Lemming)]
    Lemming = 2100,

    [Display(Name = SpeciesConstants.LittleBrownBat)]
    LittleBrownBat = 2200,

    [Display(Name = SpeciesConstants.MeadowJumpingMouse)]
    MeadowJumpingMouse = 2300,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [IsBigGame]
    [Display(Name = SpeciesConstants.Moose)]
    Moose = 2400,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [HasGameManagementArea]
    [IsBigGame]
    [Display(Name = SpeciesConstants.MountainGoat)]
    MountainGoat = 2500,

    [IsIndividualHuntable]
    [HasGameManagementArea]
    [IsBigGame]
    [Display(Name = SpeciesConstants.MuleDeer)]
    MuleDeer = 2600,

    [Display(Name = SpeciesConstants.Muskox)]
    Muskox = 2700,

    [IsTrappable]
    [Display(Name = SpeciesConstants.Muskrat)]
    Muskrat = 2800,

    [Display(Name = SpeciesConstants.NorthAmericanDeermouse)]
    NorthAmericanDeermouse = 2900,

    [Display(Name = SpeciesConstants.NorthAmericanPorcupine)]
    NorthAmericanPorcupine = 3000,

    [Display(Name = SpeciesConstants.NorthernFlyingSquirrel)]
    NorthernFlyingSquirrel = 3100,

    [Display(Name = SpeciesConstants.NorthernLongEaredBat)]
    NorthernLongEaredBat = 3200,

    [IsTrappable]
    [Display(Name = SpeciesConstants.NorthernRiverOtter)]
    NorthernRiverOtter = 3300,

    [Display(Name = SpeciesConstants.PolarBear)]
    PolarBear = 3400,

    [IsTrappable]
    [Display(Name = SpeciesConstants.RedFox)]
    RedFox = 3500,

    [IsTrappable]
    [Display(Name = SpeciesConstants.RedSquirrel)]
    RedSquirrel = 3600,

    [Display(Name = SpeciesConstants.RockPtarmigan)]
    RockPtarmigan = 3601,

    [Display(Name = SpeciesConstants.RuffedGrouse)]
    RuffedGrouse = 3602,

    [Display(Name = SpeciesConstants.SharpTailedGrouse)]
    SharpTailedGrouse = 3603,

    [Display(Name = SpeciesConstants.Shrew)]
    Shrew = 3700,

    [Display(Name = SpeciesConstants.SnowshoeHare)]
    SnowshoeHare = 3800,

    [Display(Name = SpeciesConstants.SpruceGrouse)]
    SpruceGrouse = 3801,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [HasGameManagementArea]
    [IsBigGame]
    [Display(Name = SpeciesConstants.ThinhornSheep)]
    ThinhornSheep = 3900,

    [Display(Name = SpeciesConstants.Vole)]
    Vole = 4000,

    [IsIndividualHuntable]
    [IsBigGame]
    [Display(Name = SpeciesConstants.WhiteTailedDeer)]
    WhiteTailedDeer = 4100,

    [Display(Name = SpeciesConstants.WhiteTailedPtarmigan)]
    WhiteTailedPtarmigan = 4101,

    [Display(Name = SpeciesConstants.WillowPtarmigan)]
    WillowPtarmigan = 4102,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsTrappable]
    [IsBigGame]
    [Display(Name = SpeciesConstants.Wolverine)]
    Wolverine = 4200,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [IsBigGame]
    [Display(Name = SpeciesConstants.WoodBison)]
    WoodBison = 4300,

    [Display(Name = SpeciesConstants.Woodchuck)]
    Woodchuck = 4400,

    [Display(Name = SpeciesConstants.BirdOther)]
    BirdOther = 4500
}
