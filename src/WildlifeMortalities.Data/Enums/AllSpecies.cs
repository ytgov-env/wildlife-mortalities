using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum AllSpecies
{
    Uninitialized = 0,

    [Display(Name = "American beaver")] AmericanBeaver = 1,

    [Display(Name = "American black bear")]
    AmericanBlackBear = 2,

    [Display(Name = "American martin")] AmericanMartin = 3,

    [Display(Name = "American mink")] AmericanMink = 4,

    [Display(Name = "Artic fox")] ArcticFox = 5,

    [Display(Name = "Arctic ground squirrel")]
    ArcticGroundSquirrel = 6,

    [Display(Name = "Barren-ground caribou")]
    BarrenGroundCaribou = 7,

    [Display(Name = "Bushy-tailed woodrat")]
    BushyTailedWoodrat = 8,

    [Display(Name = "Canada lynx")] CanadaLynx = 9,

    [Display(Name = "Collared pika")] CollaredPika = 10,

    [Display(Name = "Cougar")] Cougar = 11,

    [Display(Name = "Coyote")] Coyote = 12,

    [Display(Name = "Elk")] Elk = 13,

    [Display(Name = "Ermine")] Ermine = 14,

    [Display(Name = "Fisher")] Fisher = 15,

    [Display(Name = "Grey wolf")] GreyWolf = 16,

    [Display(Name = "Grizzly bear")] GrizzlyBear = 17,

    [Display(Name = "Hoary marmot")] HoaryMarmot = 18,

    [Display(Name = "Least chipmunk")] LeastChipmunk = 19,

    [Display(Name = "Least weasel")] LeastWeasel = 20,

    [Display(Name = "Lemming")] Lemming = 21,

    [Display(Name = "Little brown bat")] LittleBrownBat = 22,

    [Display(Name = "Meadow jumping mouse")]
    MeadowJumpingMouse = 23,

    [Display(Name = "Moose")] Moose = 24,

    [Display(Name = "Mountain goat")] MountainGoat = 25,

    [Display(Name = "Mule deer")] MuleDeer = 26,

    [Display(Name = "Muskox")] Muskox = 27,

    [Display(Name = "Muskrat")] Muskrat = 28,

    [Display(Name = "North american deermouse")]
    NorthAmericanDeermouse = 29,

    [Display(Name = "North american porcupine")]
    NorthAmericanPorcupine = 30,

    [Display(Name = "Northern flying squirrel")]
    NorthernFlyingSquirrel = 31,

    [Display(Name = "Northern long-eared bat")]
    NorthernLongEaredBat = 32,

    [Display(Name = "Northern river otter")]
    NorthernRiverOtter = 33,

    [Display(Name = "Polar bear")] PolarBear = 34,

    [Display(Name = "Red fox")] RedFox = 35,

    [Display(Name = "Red squirrel")] RedSquirrel = 36,

    [Display(Name = "Shrew")] Shrew = 37,

    [Display(Name = "Snowshoe hare")] SnowshoeHare = 38,

    [Display(Name = "Thinhorn sheep")] ThinhornSheep = 39,

    [Display(Name = "Vole")] Vole = 40,

    [Display(Name = "White-tailed deer")] WhiteTailedDeer = 41,

    [Display(Name = "Wolverine")] Wolverine = 42,

    [Display(Name = "Wood bison")] WoodBison = 43,

    [Display(Name = "Woodchuck")] Woodchuck = 44,

    [Display(Name = "Woodland caribou")] WoodlandCaribou = 45,

    [Display(Name = "Bird")] Bird = 46
}
