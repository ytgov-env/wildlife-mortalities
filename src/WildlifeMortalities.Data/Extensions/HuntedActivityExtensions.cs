using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using static WildlifeMortalities.Data.Entities.Mortalities.CaribouMortality;

namespace WildlifeMortalities.Data.Extensions;

public static class HuntedActivityExtensions
{
    public static CaribouHerd? GetLegalHerd(this HuntedActivity activity) =>
        activity.GameManagementArea.GetLegalHerd(activity.Mortality.DateOfDeath!.Value);

    public static bool IsPorcupineCaribou(this HuntedActivity activity) =>
        activity.GetLegalHerd() == CaribouHerd.Porcupine;
}
