using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class WolverineMortality : Mortality, IHasBioSubmission
{
    public override Species Species => Species.Wolverine;
    public WolverineBioSubmission? BioSubmission { get; set; }

    public BioSubmission CreateDefaultBioSubmission() => new WolverineBioSubmission(this);
}
