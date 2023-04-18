using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class MooseMortality : Mortality, IHasBioSubmission
{
    public override Species Species => Species.Moose;
    public MooseBioSubmission? BioSubmission { get; set; }

    public BioSubmission CreateDefaultBioSubmission() => new MooseBioSubmission(this);
}
