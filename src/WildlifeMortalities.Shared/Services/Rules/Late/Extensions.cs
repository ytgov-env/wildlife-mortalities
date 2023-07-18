using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Shared.Services.Rules.Late;

public static class Extensions
{
    public static DateTimeOffset GetTimestampAfterKill(this HarvestActivity activity, int hours) =>
        activity.Mortality.DateOfDeath!.Value.AddDays(1).AddSeconds(-1).AddHours(hours);

    public static DateTimeOffset OccuredMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(
        this HarvestActivity activity
    )
    {
        var dateOfDeath = activity.Mortality.DateOfDeath!.Value;
        var lastDayOfMonth = new DateTimeOffset(
            dateOfDeath.Year,
            dateOfDeath.Month,
            1,
            0,
            0,
            0,
            dateOfDeath.Offset
        )
            .AddMonths(1)
            .AddSeconds(-1);
        return lastDayOfMonth.AddDays(15);
    }

    public static DateTimeOffset OccuredMoreThanFifteenDaysAfterTheEndOfTheTrappingSeasonForSpecies(
        this TrappedActivity activity
    )
    {
        return activity.ActivityQueueItem.BagLimitEntry.PeriodEnd.AddDays(16).AddSeconds(-1);
    }
}
