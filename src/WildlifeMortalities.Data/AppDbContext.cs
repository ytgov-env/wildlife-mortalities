using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Licences;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reporters;

namespace WildlifeMortalities.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Reporter> Reporters => Set<Reporter>();

    public DbSet<Licence> Licences => Set<Licence>();
    public DbSet<Seal> Seals => Set<Seal>();

    public DbSet<Mortality> Mortalities => Set<Mortality>();
    public DbSet<HarvestReport> HarvestReports => Set<HarvestReport>();

    public DbSet<BioSubmission> BioSubmissions => Set<BioSubmission>();

    public DbSet<GameManagementArea> GameManagementAreas => Set<GameManagementArea>();
    public DbSet<GameManagementAreaSpecies> GameManagementAreaSpecies => Set<GameManagementAreaSpecies>();
    public DbSet<GameManagementAreaSchedule> GameManagementAreaSchedules => Set<GameManagementAreaSchedule>();
    public DbSet<GameManagementUnit> GameManagementUnits => Set<GameManagementUnit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Seal>().ToTable("Seals");
        modelBuilder.Entity<Mortality>().ToTable("Mortalities");

        modelBuilder.Entity<Seal>().Property(s => s.Species).HasConversion<string>();

        modelBuilder.Entity<TrappedHarvestReport>().HasMany(t => t.Mortalities).WithOne();
        modelBuilder.Entity<HuntedHarvestReport>().HasOne(t => t.Mortality).WithOne();

        modelBuilder.Entity<Mortality>().HasOne(m => m.Reporter)
            .WithMany(r => r.Mortalities).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<WoodBisonMortality>(b =>
        {
            b.Property(b => b.TemporarySealNumber).HasColumnName(nameof(WoodBisonMortality.TemporarySealNumber));
            b.Property(b => b.Sex).HasConversion<string>().HasColumnName(nameof(WoodBisonMortality.Sex));
            b.Property(b => b.GameManagementAreaId).HasColumnName(nameof(WoodBisonMortality.GameManagementAreaId));
            b.Property(b => b.Landmark).HasColumnName(nameof(WoodBisonMortality.Landmark));
            b.Property(b => b.Coordinates).HasColumnName(nameof(WoodBisonMortality.Coordinates));
        });

        modelBuilder.Entity<AmericanBlackBearMortality>(b =>
        {
            b.Property(b => b.TemporarySealNumber).HasColumnName(nameof(AmericanBlackBearMortality.TemporarySealNumber));
            b.Property(b => b.Sex).HasConversion<string>().HasColumnName(nameof(AmericanBlackBearMortality.Sex));
            b.Property(b => b.GameManagementAreaId).HasColumnName(nameof(AmericanBlackBearMortality.GameManagementAreaId));
            b.Property(b => b.Landmark).HasColumnName(nameof(AmericanBlackBearMortality.Landmark));
            b.Property(b => b.Coordinates).HasColumnName(nameof(AmericanBlackBearMortality.Coordinates));
        });

        modelBuilder.Entity<GameManagementArea>().Property(a => a.Area).HasComputedColumnSql("[Zone] + [Subzone]", true);
        modelBuilder.Entity<GameManagementAreaSpecies>().Property(s => s.Species).HasConversion<string>().HasMaxLength(25);
        modelBuilder.Entity<GameManagementAreaSchedule>().Property(s => s.PeriodStart).HasColumnType("date");
        modelBuilder.Entity<GameManagementAreaSchedule>().Property(s => s.PeriodEnd).HasColumnType("date");
        modelBuilder.Entity<GameManagementUnit>().Property(u => u.ActiveFrom).HasColumnType("date");
        modelBuilder.Entity<GameManagementUnit>().Property(u => u.ActiveTo).HasColumnType("date");
        modelBuilder.Entity<GameManagementUnit>().Property(u => u.Name).HasMaxLength(50);

        // These shadow properties are referenced during ETL to sync licence and seal data from their source (POSSE)
        modelBuilder.Entity<Licence>().Property<int?>("PosseId");
        modelBuilder.Entity<Seal>().Property<int?>("PosseId");

        base.OnModelCreating(modelBuilder);
    }
}
