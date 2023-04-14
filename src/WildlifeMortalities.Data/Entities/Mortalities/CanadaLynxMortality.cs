using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class CanadaLynxMortality : Mortality, IHasBioSubmission
{
    public CanadaLynxBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.CanadaLynx;

    public BioSubmission CreateDefaultBioSubmission() => new CanadaLynxBioSubmission(this);
}
