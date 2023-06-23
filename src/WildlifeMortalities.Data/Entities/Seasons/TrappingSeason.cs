using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;

namespace WildlifeMortalities.Data.Entities.Seasons;

public class TrappingSeason : Season
{
    private TrappingSeason() { }

    public TrappingSeason(int startYear)
    {
        StartDate = new DateTimeOffset(startYear, 7, 1, 0, 0, 0, TimeSpan.FromHours(-7));
        EndDate = new DateTimeOffset(startYear + 1, 6, 30, 23, 59, 59, TimeSpan.FromHours(-7));
        FriendlyName = ToString();
    }

    public List<TrappingBagLimitEntry> BagLimitEntries { get; set; } = null!;

    public static async Task<TrappingSeason> GetSeason(Report report, AppDbContext context) =>
        await GetSeason<TrappingSeason>(report, context);

    public static async Task<TrappingSeason?> TryGetSeason(
        Authorization authorization,
        AppDbContext context
    ) => await TryGetSeason<TrappingSeason>(authorization, context);

    public override string ToString()
    {
        return $"{StartDate.Year % 100:00}/{EndDate.Year % 100:00}";
    }
}

public class TrappingSeasonConfig : SeasonConfig<TrappingSeason> { }
