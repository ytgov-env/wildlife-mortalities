using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class CanadaLynxMortality : Mortality, IHasBioSubmission
{
    public int? FurbearerSealingCertificateId { get; set; }
    public FurbearerSealingCertificate? FurbearerSealingCertificate { get; set; }
    public CanadaLynxBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.CanadaLynx;
}
