using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities;

public abstract class Season
{
    public int Id { get; set; }
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
    public List<Report> Reports { get; set; } = null!;

    public static async Task<TSeason> GetSeason<TSeason>(Report report, AppDbContext context)
        where TSeason : Season
    {
        var date = report.GetRelevantDateForSeason();

        var season = await context.Seasons
            .OfType<TSeason>()
            .SingleOrDefaultAsync(x => date >= x.StartDate && date <= x.EndDate);

        return season
            ?? throw new ArgumentException(
                $"Unable to resolve season for date {date}",
                nameof(report)
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
