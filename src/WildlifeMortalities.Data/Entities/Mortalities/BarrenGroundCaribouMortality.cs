namespace WildlifeMortalities.Data.Entities.Mortalities;

public class BarrenGroundCaribouMortality : Mortality
{
    public BarrenGroundCaribouHerd Herd { get; set; }
    public enum BarrenGroundCaribouHerd
    {
        Uninitialized = 0,
        Fortymile,
        Nelchina,
        Porcupine
    }
}
