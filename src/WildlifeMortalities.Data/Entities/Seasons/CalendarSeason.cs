using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Seasons;

public class CalendarSeason : Season
{
    private CalendarSeason() { }

    public CalendarSeason(int startYear)
    {
        StartDate = new DateTimeOffset(startYear, 1, 1, 0, 0, 0, TimeSpan.FromHours(-7));
        EndDate = new DateTimeOffset(startYear, 12, 31, 23, 59, 59, TimeSpan.FromHours(-7));
        FriendlyName = ToString();
    }

    public static async Task<CalendarSeason> GetSeason(Activity activity, AppDbContext context) =>
        await GetSeason<CalendarSeason>(activity, context);

    public static async Task<CalendarSeason> GetSeason(Report report, AppDbContext context) =>
        await GetSeason<CalendarSeason>(report, context);

    public static async Task<CalendarSeason?> TryGetSeason(
        Authorization authorization,
        AppDbContext context
    ) => await TryGetSeason<CalendarSeason>(authorization, context);

    public override string ToString()
    {
        return StartDate.Year.ToString();
    }
}

public class CalendarSeasonConfig : SeasonConfig<CalendarSeason> { }
