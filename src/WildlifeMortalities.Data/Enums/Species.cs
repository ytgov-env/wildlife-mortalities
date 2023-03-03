using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public class IsOutfitterGuidedHuntableAttribute : Attribute { }

public class IsSpecialGuidedHuntableAttribute : Attribute { }

public class IsIndividualHuntableAttribute : Attribute { }

public class IsTrappableAttribute : Attribute { }

public class HasGameManagementAreaAttribute : Attribute { }

public enum Species
{
    [IsTrappable]
    [Display(Name = "Beaver")]
    AmericanBeaver = 10,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = "Black bear")]
    AmericanBlackBear = 20,

    [IsTrappable]
    [Display(Name = "Martin")]
    AmericanMartin = 30,

    [IsTrappable]
    [Display(Name = "Mink")]
    AmericanMink = 40,

    [IsTrappable]
    [Display(Name = "Arctic fox")]
    ArcticFox = 50,

    [Display(Name = "Arctic ground squirrel")]
    ArcticGroundSquirrel = 60,

    [Display(Name = "Bushy-tailed woodrat")]
    BushyTailedWoodrat = 70,

    [IsTrappable]
    [Display(Name = "Lynx")]
    CanadaLynx = 80,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = "Caribou")]
    Caribou = 90,

    [Display(Name = "Pika")]
    CollaredPika = 100,

    [Display(Name = "Cougar")]
    Cougar = 110,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [IsTrappable]
    [Display(Name = "Coyote")]
    Coyote = 120,

    [IsIndividualHuntable]
    [Display(Name = "Elk")]
    Elk = 130,

    [IsTrappable]
    [Display(Name = "Ermine")]
    Ermine = 140,

    [IsTrappable]
    [Display(Name = "Fisher")]
    Fisher = 150,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [IsTrappable]
    [Display(Name = "Wolf")]
    GreyWolf = 160,

    [Display(Name = "Grouse")]
    Grouse = 161,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = "Grizzly bear")]
    GrizzlyBear = 170,

    [Display(Name = "Marmot")]
    HoaryMarmot = 180,

    [Display(Name = "Chipmunk")]
    LeastChipmunk = 190,

    [IsTrappable]
    [Display(Name = "Weasel")]
    LeastWeasel = 200,

    [Display(Name = "Lemming")]
    Lemming = 210,

    [Display(Name = "Little brown bat")]
    LittleBrownBat = 220,

    [Display(Name = "Meadow jumping mouse")]
    MeadowJumpingMouse = 230,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = "Moose")]
    Moose = 240,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = "Goat")]
    MountainGoat = 250,

    [IsIndividualHuntable]
    [HasGameManagementArea]
    [Display(Name = "Mule deer")]
    MuleDeer = 260,

    [Display(Name = "Muskox")]
    Muskox = 270,

    [IsTrappable]
    [Display(Name = "Muskrat")]
    Muskrat = 280,

    [Display(Name = "North american deermouse")]
    NorthAmericanDeermouse = 290,

    [Display(Name = "North american porcupine")]
    NorthAmericanPorcupine = 300,

    [Display(Name = "Northern flying squirrel")]
    NorthernFlyingSquirrel = 310,

    [Display(Name = "Northern long-eared bat")]
    NorthernLongEaredBat = 320,

    [IsTrappable]
    [Display(Name = "Otter")]
    NorthernRiverOtter = 330,

    [Display(Name = "Polar bear")]
    PolarBear = 340,

    [Display(Name = "Ptarmigan")]
    Ptarmigan = 341,

    [IsTrappable]
    [Display(Name = "Red fox")]
    RedFox = 350,

    [IsTrappable]
    [Display(Name = "Red squirrel")]
    RedSquirrel = 360,

    [Display(Name = "Shrew")]
    Shrew = 370,

    [Display(Name = "Snowshoe hare")]
    SnowshoeHare = 380,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = "Sheep")]
    ThinhornSheep = 390,

    [Display(Name = "Vole")]
    Vole = 400,

    [IsIndividualHuntable]
    [Display(Name = "White-tailed deer")]
    WhiteTailedDeer = 410,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsTrappable]
    [Display(Name = "Wolverine")]
    Wolverine = 420,

    [IsIndividualHuntable]
    [IsOutfitterGuidedHuntable]
    [IsSpecialGuidedHuntable]
    [HasGameManagementArea]
    [Display(Name = "Bison")]
    WoodBison = 430,

    [Display(Name = "Woodchuck")]
    Woodchuck = 440,

    [Display(Name = "Bird (other)")]
    BirdOther = 450
}
