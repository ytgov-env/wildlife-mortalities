using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities;

public class FurbearerSealingCertificate
{
    public int Id { get; set; }
    public string HumanReadableId { get; set; } = string.Empty;
    public List<Mortality> Mortalities { get; set; } = null!;
}
