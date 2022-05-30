namespace WildlifeMortalities.Data.Entities.Mortalities;

public class GreyWolfMortality : Mortality
{
    public int? TrappedHarvestReportId { get; set; }
    public TrappedHarvestReport? TrappedHarvestReport { get; set; }
}
