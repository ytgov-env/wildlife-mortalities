using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class BarrenGroundCaribouBioSubmission : BioSubmission
{
    public int MortalityId { get; set; }
    public BarrenGroundCaribouMortality Mortality { get; set; }
}
