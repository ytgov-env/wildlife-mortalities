using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class MuleDeerMortality : Mortality
{
    public MuleDeerBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.MuleDeer;
}
