using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class AmericanBlackBearBioSubmission : BioSubmission
{
    public int MortalityId { get; set; }
    public AmericanBlackBearMortality Mortality { get; set; } = null!;
}
