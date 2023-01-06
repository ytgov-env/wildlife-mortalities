using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class GrizzlyBearBioSubmission
{
    public string SkullCondition { get; set; } = string.Empty;
    public int SkullLengthMillimetres { get; set; }
    public int SkullHeightMillimetres { get; set; }
    public bool IsEvidenceOfSexAttached { get; set; }
    public int MortalityId { get; set; }
    public GrizzlyBearMortality Mortality { get; set; } = null!;
}
