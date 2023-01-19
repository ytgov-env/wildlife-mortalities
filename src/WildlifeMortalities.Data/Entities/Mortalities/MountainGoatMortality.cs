using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class MountainGoatMortality : Mortality, IHasBioSubmission
{
    public MountainGoatBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.MountainGoat;
}
