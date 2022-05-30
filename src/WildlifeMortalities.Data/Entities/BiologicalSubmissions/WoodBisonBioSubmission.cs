using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WoodBisonBioSubmission : BioSubmission
{
    public int MortalityId { get; set; }
    public WoodBisonMortality Mortality { get; set; }
}
