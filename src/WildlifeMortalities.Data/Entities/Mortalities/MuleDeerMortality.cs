using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class MuleDeerMortality : Mortality, IHasBioSubmission
{
    public MuleDeerBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.MuleDeer;

    public BioSubmission CreateDefaultBioSubmission() => new MuleDeerBioSubmission(this);
}
