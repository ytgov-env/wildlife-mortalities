using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Authorizations;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities;

public abstract class Season
{
    public int Id { get; set; }
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }

    public string FriendlyName { get; set; } = string.Empty;
    public List<Report> Reports { get; set; } = null!;
    public List<Authorization> Authorizations { get; set; } = null!;

    public static async Task<TSeason> GetSeason<TSeason>(Report report, AppDbContext context)
        where TSeason : Season
    {
        var date = report.GetRelevantDateForSeason();

        return await context.Seasons
            .OfType<TSeason>()
            .SingleAsync(x => date >= x.StartDate && date <= x.EndDate);
    }

    public static async Task<TSeason?> TryGetSeason<TSeason>(
        Authorization authorization,
        AppDbContext context
    )
        where TSeason : Season
    {
        return await context.Seasons
            .OfType<TSeason>()
            .SingleOrDefaultAsync(
                x =>
                    authorization.ValidFromDateTime >= x.StartDate
                    && authorization.ValidToDateTime <= x.EndDate
            );
    }

    public bool IsValidSubset(DateTimeOffset periodStart, DateTimeOffset periodEnd)
    {
        return periodStart >= StartDate && periodEnd <= EndDate;
    }
}

public class SeasonConfig<T> : IEntityTypeConfiguration<T>
    where T : Season
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .ToTable(TableNameConstants.Seasons)
            .HasIndex(s => new { s.StartDate, s.EndDate })
            .IsUnique();
        builder.HasMany(s => s.Reports).WithOne(r => (T)r.Season);
    }
}
