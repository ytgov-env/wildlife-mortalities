using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class GreyWolfMortality : Mortality, IHasBioSubmission
{
    public GreyWolfBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.GreyWolf;

    public BioSubmission CreateDefaultBioSubmission() => new GreyWolfBioSubmission(this);
}
