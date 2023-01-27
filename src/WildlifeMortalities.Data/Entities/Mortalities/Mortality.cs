using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public abstract class Mortality<T> : Mortality where T : Mortality
{
    public virtual IEntityTypeConfiguration<T> GetConfig() => new MortalityConfig<T>();
}

public abstract class Mortality
{
    public int Id { get; set; }
    public int ReportId { get; set; }
    public Report Report { get; set; } = null!;
    public DateTime? DateOfDeath { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public Sex Sex { get; set; }
    public string Discriminator { get; set; } = null!;
    public abstract Species Species { get; }
}

public class MortalityConfig<T> : IEntityTypeConfiguration<T> where T : Mortality
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable("Mortalities");
        builder.Property(m => m.Latitude).HasPrecision(10, 8);
        builder.Property(m => m.Longitude).HasPrecision(11, 8);
        builder.Ignore(m => m.Species);
    }
}
