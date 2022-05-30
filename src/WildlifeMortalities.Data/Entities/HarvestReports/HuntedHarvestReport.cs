using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities;

public class HuntedHarvestReport : HarvestReport
{
    public int MortalityId { get; set; }
    public Mortality Mortality { get; set; }
    public string? TemporarySealNumber { get; set; }
    public int? SealId { get; set; }
    public Seal? Seal { get; set; }
}
