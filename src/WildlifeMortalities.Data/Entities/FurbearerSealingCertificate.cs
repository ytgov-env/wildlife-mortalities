using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Data.Entities;

public class FurbearerSealingCertificate
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public List<MortalityReport> MortalityReports { get; set; } = null!;
}
