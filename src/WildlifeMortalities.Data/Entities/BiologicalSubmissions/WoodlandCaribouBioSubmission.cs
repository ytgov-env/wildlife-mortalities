using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WoodlandCaribouBioSubmission : BioSubmission
{
    public int MortalityId { get; set; }
    public WoodlandCaribouMortality Mortality { get; set; }
}
