using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities;

public abstract class Season
{
    public int Id { get; set; }
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }

    public abstract Season GetSeason(Report report);
}

public class SeasonConfig<T> : IEntityTypeConfiguration<T>
    where T : Season
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable("Seasons").HasIndex(s => new { s.StartDate, s.EndDate }).IsUnique();
    }
}
