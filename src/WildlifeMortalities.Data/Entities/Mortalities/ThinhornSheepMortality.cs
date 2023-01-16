namespace WildlifeMortalities.Data.Entities.Mortalities;

public class ThinhornSheepMortality : Mortality<ThinhornSheepMortality>
{
    public ThinhornSheepBodyColour BodyColour { get; set; }
    public ThinhornSheepTailColour TailColour { get; set; }

    public override Species Species => Species.ThinhornSheep;
}

public enum ThinhornSheepBodyColour
{
    Dark = 10,
    Fannin = 20,
    White = 30
}

public enum ThinhornSheepTailColour
{
    Dark = 10,
    White = 20
}
