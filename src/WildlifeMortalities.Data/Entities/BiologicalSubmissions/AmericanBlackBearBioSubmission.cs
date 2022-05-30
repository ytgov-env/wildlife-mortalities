using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class AmericanBlackBearBioSubmission
{
    public int MortalityId { get; set; }
    public AmericanBlackBearMortality Mortality { get; set; }
}
