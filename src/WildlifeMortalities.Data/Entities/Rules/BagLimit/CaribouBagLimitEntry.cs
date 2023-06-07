using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using static WildlifeMortalities.Data.Entities.Mortalities.CaribouMortality;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class CaribouBagLimitEntry : BagLimitEntry
{
    public CaribouBagLimitEntry()
    {
        Species = Species.Caribou;
    }

    public CaribouHerd Herd { get; set; }

    override public bool Matches(HuntedActivity activity, Season season)
    {
        var baseResult = base.Matches(activity, season);
        if (!baseResult)
            return false;

        if (activity.Mortality is not CaribouMortality caribouMortality)
            return false;

        return caribouMortality.Herd == Herd;
    }
}
