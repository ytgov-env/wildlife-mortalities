using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class GreyWolfMortality : Mortality, IHasBioSubmission
{
    public int? FurbearerSealingCertificateId { get; set; }
    public FurbearerSealingCertificate? FurbearerSealingCertificate { get; set; }
    public GreyWolfBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.GreyWolf;
}
