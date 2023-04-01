using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Seasons;

public class CalendarSeason : Season
{
    private CalendarSeason() { }

    public CalendarSeason(int startYear)
    {
        StartDate = new DateTimeOffset(startYear, 1, 1, 0, 0, 0, TimeSpan.FromHours(-7));
        EndDate = new DateTimeOffset(startYear, 12, 31, 23, 59, 59, TimeSpan.FromHours(-7));
    }

    public override CalendarSeason GetSeason(Report report)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return StartDate.Year.ToString();
    }
}

public class CalendarSeasonConfig : SeasonConfig<CalendarSeason> { }
