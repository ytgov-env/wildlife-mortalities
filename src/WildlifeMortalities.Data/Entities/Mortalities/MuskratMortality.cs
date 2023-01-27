namespace WildlifeMortalities.Data.Entities.Mortalities;

public class MuskratMortality : Mortality
{
    public int? FurbearerSealingCertificateId { get; set; }
    public FurbearerSealingCertificate? FurbearerSealingCertificate { get; set; }
    public override Species Species => Species.Muskrat;
}
