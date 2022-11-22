using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum AllSpecies
{
    [Display(Name = "American beaver")] AmericanBeaver = 10,

    [Display(Name = "American black bear")]
    AmericanBlackBear = 20,

    [Display(Name = "American martin")] AmericanMartin = 30,

    [Display(Name = "American mink")] AmericanMink = 40,

    [Display(Name = "Arctic fox")] ArcticFox = 50,

    [Display(Name = "Arctic ground squirrel")]
    ArcticGroundSquirrel = 60,

    [Display(Name = "Bushy-tailed woodrat")]
    BushyTailedWoodrat = 70,

    [Display(Name = "Canada lynx")] CanadaLynx = 80,

    [Display(Name = "Caribou")] Caribou = 90,

    [Display(Name = "Collared pika")] CollaredPika = 100,

    [Display(Name = "Cougar")] Cougar = 110,

    [Display(Name = "Coyote")] Coyote = 120,

    [Display(Name = "Elk")] Elk = 130,

    [Display(Name = "Ermine")] Ermine = 140,

    [Display(Name = "Fisher")] Fisher = 150,

    [Display(Name = "Grey wolf")] GreyWolf = 160,

    [Display(Name = "Grizzly bear")] GrizzlyBear = 170,

    [Display(Name = "Hoary marmot")] HoaryMarmot = 180,

    [Display(Name = "Least chipmunk")] LeastChipmunk = 190,

    [Display(Name = "Least weasel")] LeastWeasel = 200,

    [Display(Name = "Lemming")] Lemming = 210,

    [Display(Name = "Little brown bat")] LittleBrownBat = 220,

    [Display(Name = "Meadow jumping mouse")]
    MeadowJumpingMouse = 230,

    [Display(Name = "Moose")] Moose = 240,

    [Display(Name = "Mountain goat")] MountainGoat = 250,

    [Display(Name = "Mule deer")] MuleDeer = 260,

    [Display(Name = "Muskox")] Muskox = 270,

    [Display(Name = "Muskrat")] Muskrat = 280,

    [Display(Name = "North american deermouse")]
    NorthAmericanDeermouse = 290,

    [Display(Name = "North american porcupine")]
    NorthAmericanPorcupine = 300,

    [Display(Name = "Northern flying squirrel")]
    NorthernFlyingSquirrel = 310,

    [Display(Name = "Northern long-eared bat")]
    NorthernLongEaredBat = 320,

    [Display(Name = "Northern river otter")]
    NorthernRiverOtter = 330,

    [Display(Name = "Polar bear")] PolarBear = 340,

    [Display(Name = "Red fox")] RedFox = 350,

    [Display(Name = "Red squirrel")] RedSquirrel = 360,

    [Display(Name = "Shrew")] Shrew = 370,

    [Display(Name = "Snowshoe hare")] SnowshoeHare = 380,

    [Display(Name = "Thinhorn sheep")] ThinhornSheep = 390,

    [Display(Name = "Vole")] Vole = 400,

    [Display(Name = "White-tailed deer")] WhiteTailedDeer = 410,

    [Display(Name = "Wolverine")] Wolverine = 420,

    [Display(Name = "Wood bison")] WoodBison = 430,

    [Display(Name = "Woodchuck")] Woodchuck = 440,

    [Display(Name = "Bird")] Bird = 450
}
