namespace WildlifeMortalities.Data.Entities.Mortalities;

public class ErmineMortality : Mortality
{
    public int? FurbearerSealingCertificateId { get; set; }
    public FurbearerSealingCertificate? FurbearerSealingCertificate { get; set; }
    public override Species Species => Species.Ermine;
}
