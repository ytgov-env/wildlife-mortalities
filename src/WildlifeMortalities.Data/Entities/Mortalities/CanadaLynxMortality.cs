using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class CanadaLynxMortality : Mortality
{
    public CanadaLynxBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.CanadaLynx;
}
