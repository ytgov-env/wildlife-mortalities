using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class AmericanBlackBearBioSubmission : BioSubmission
{
    public string SkullCondition { get; set; } = "";
    public int SkullLengthMillimetre { get; set; }
    public int SkullHeightMillimetre { get; set; }
    public int MortalityId { get; set; }
    public AmericanBlackBearMortality Mortality { get; set; } = null!;
}
