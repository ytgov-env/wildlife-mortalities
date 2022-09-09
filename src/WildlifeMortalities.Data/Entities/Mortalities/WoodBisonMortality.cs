namespace WildlifeMortalities.Data.Entities.Mortalities;

public class WoodBisonMortality : Mortality
{
    public PregnancyStatus PregnancyStatus { get; set; }
    public bool IsWounded { get; set; }
}

public enum PregnancyStatus
{
    Uninitialized = 0,
    False = 1,
    True = 2,
    Unknown = 3
}
