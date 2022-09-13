namespace WildlifeMortalities.Data.Entities.Mortalities;

public class ThinhornSheepMortality : Mortality
{
    public ThinhornSheepBodyColour BodyColour { get; set; }
    public ThinhornSheepTailColour TailColour { get; set; }
}

public enum ThinhornSheepBodyColour
{
    Uninitialized = 0,
    Dark = 1,
    Fannin = 2,
    White = 3
}

public enum ThinhornSheepTailColour
{
    Uninitialized = 0,
    Dark = 1,
    White = 2
}
