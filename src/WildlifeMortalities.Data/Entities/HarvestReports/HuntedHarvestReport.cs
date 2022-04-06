namespace WildlifeMortalities.Data.Entities;

public class HuntedHarvestReport : HarvestReportBase
{
    public HuntedMortality Mortality { get; set; } = null!;
}
