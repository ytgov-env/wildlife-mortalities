using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class GreyWolfMortality : Mortality
{
    public GreyWolfBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.GreyWolf;
}
