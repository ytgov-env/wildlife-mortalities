using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class NeckSnaredGreyWolfTrappingBagLimitEntry : TrappingBagLimitEntry
{
    override public bool Matches(HarvestActivity activity, Report report)
    {
        var baseResult = base.Matches(activity, report);
        if (!baseResult)
            return false;

        if (activity.Mortality is not GreyWolfMortality)
            return false;

        if (activity is not TrappedActivity trappedActivity)
            return false;

        return trappedActivity.HarvestMethod == TrappedActivity.HarvestMethodType.NeckSnare;
    }
}
