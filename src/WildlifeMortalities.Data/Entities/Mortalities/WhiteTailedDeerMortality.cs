using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class WhiteTailedDeerMortality : Mortality
{
    public WhiteTailedDeerBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.WhiteTailedDeer;
}
