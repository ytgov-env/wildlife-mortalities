namespace WildlifeMortalities.Data.Entities.Mortalities;

public class WolverineMortality : Mortality
{
    public int? TrappedHarvestReportId { get; set; }
    public TrappedHarvestReport? TrappedHarvestReport { get; set; }
}
