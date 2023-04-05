using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.Data.Entities;

public abstract class Season
{
    public int Id { get; set; }
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }

    public string FriendlyName { get; set; } = string.Empty;
    public List<Report> Reports { get; set; } = null!;
    public List<Authorization> Authorizations { get; set; } = null!;

    public static async Task<TSeason?> GetSeason<TSeason>(Report report, AppDbContext context)
        where TSeason : Season
    {
        var date = report.GetRelevantDateForSeason();

        return await context.Seasons
            .OfType<TSeason>()
            .SingleOrDefaultAsync(x => date >= x.StartDate && date <= x.EndDate);
    }

    public static async Task<TSeason?> GetSeason<TSeason>(
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
}

public class SeasonConfig<T> : IEntityTypeConfiguration<T>
    where T : Season
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable("Seasons").HasIndex(s => new { s.StartDate, s.EndDate }).IsUnique();
        builder.HasMany(s => s.Reports).WithOne(r => (T)r.Season);
    }
}
