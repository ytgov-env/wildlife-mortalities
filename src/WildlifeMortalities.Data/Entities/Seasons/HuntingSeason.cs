using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Seasons;

public class HuntingSeason : Season
{
    private HuntingSeason() { }

    public HuntingSeason(int startYear)
    {
        StartDate = new DateTimeOffset(startYear, 4, 1, 0, 0, 0, TimeSpan.FromHours(-7));
        EndDate = new DateTimeOffset(startYear + 1, 3, 31, 23, 59, 59, TimeSpan.FromHours(-7));
    }

    public override HuntingSeason GetSeason(Report report)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"{StartDate.Year % 100:00}/{EndDate.Year % 100:00}";
    }
}

public class HuntingSeasonConfig : SeasonConfig<HuntingSeason> { }
