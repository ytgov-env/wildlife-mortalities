using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class WhiteTailedDeerMortality : Mortality, IHasBioSubmission
{
    public WhiteTailedDeerBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.WhiteTailedDeer;

    public BioSubmission CreateDefaultBioSubmission() => new WhiteTailedDeerBioSubmission(this);
}
