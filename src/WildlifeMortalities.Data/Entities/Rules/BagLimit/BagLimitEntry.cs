using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class BagLimitEntry
{
    public int Id { get; set; }
    public List<GameManagementArea> Areas { get; set; } = null!;
    public Species Species { get; set; }
    public Sex? Sex { get; set; }
    public Season Season { get; set; } = null!;
    public DateTimeOffset PeriodStart { get; set; }
    public DateTimeOffset PeriodEnd { get; set; }
    public List<BagLimitEntry> SharedWithSpecies { get; set; } = null!;
    public int MaxValue { get; set; }

    public virtual bool Matches(HuntedActivity activity, Season season)
    {
        return Areas.Any(x => x.Id == activity.GameManagementArea.Id)
            && Species == activity.Mortality.Species
            && Season.Id == season.Id
            && PeriodStart <= activity.Mortality.DateOfDeath
            && PeriodEnd >= activity.Mortality.DateOfDeath
            && (!Sex.HasValue || Sex == activity.Mortality.Sex);
    }
}
