using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class BagLimitEntry
{
    public int Id { get; set; }
    public GameManagementArea Area { get; set; } = null!;
    public Species Species { get; set; }
    public Sex? Sex { get; set; }
    public Season Season { get; set; } = null!;
    public IEnumerable<BagLimitEntry> SharedWith { get; set; } = null!;

    public int MaxValue { get; set; }

    public virtual bool Matches(HuntedActivity activity, Season season)
    {
        return Area.Id == activity.GameManagementArea.Id
            && Species == activity.Mortality.Species
            && Season.Id == season.Id
            && (!Sex.HasValue || Sex == activity.Mortality.Sex);
    }
}
