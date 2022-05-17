using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities;

public class TrappedHarvestReport : HarvestReport
{
    public List<Mortality> Mortalities { get; set; } = new();
}
