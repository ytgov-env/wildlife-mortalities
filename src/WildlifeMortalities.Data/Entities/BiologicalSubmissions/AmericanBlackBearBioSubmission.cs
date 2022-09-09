using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class AmericanBlackBearBioSubmission : BioSubmission
{
    public string SkullCondition { get; set; } = "";
    public decimal SkullLength { get; set; }
    public decimal SkullHeight { get; set; }
    public int MortalityId { get; set; }
    public AmericanBlackBearMortality Mortality { get; set; } = null!;
}
