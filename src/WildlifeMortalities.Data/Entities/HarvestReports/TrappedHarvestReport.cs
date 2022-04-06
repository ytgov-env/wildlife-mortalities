namespace WildlifeMortalities.Data.Entities;

public class TrappedHarvestReport : HarvestReportBase
{
    public List<TrappedMortality> Mortalities { get; set; } = new();
}
