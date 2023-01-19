using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class WoodBisonMortality : Mortality, IHasBioSubmission
{
    public PregnancyStatus PregnancyStatus { get; set; }
    public bool IsWounded { get; set; }
    public WoodBisonBioSubmission? BioSubmission { get; set; }

    public override Species Species => Species.WoodBison;
}

public enum PregnancyStatus
{
    False = 10,
    True = 20,
    Unknown = 30
}
