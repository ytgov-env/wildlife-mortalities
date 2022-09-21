using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Person> People => Set<Person>();

    public DbSet<Authorization> Authorizations => Set<Authorization>();
    public DbSet<Seal> Seals => Set<Seal>();

    public DbSet<Mortality> Mortalities => Set<Mortality>();
    public DbSet<MortalityReport> MortalityReports => Set<MortalityReport>();
    public DbSet<Violation> Violations => Set<Violation>();

    public DbSet<BioSubmission> BioSubmissions => Set<BioSubmission>();

    public DbSet<GameManagementArea> GameManagementAreas => Set<GameManagementArea>();

    public DbSet<GameManagementAreaSpecies> GameManagementAreaSpecies =>
        Set<GameManagementAreaSpecies>();

    public DbSet<GameManagementAreaSchedule> GameManagementAreaSchedules =>
        Set<GameManagementAreaSchedule>();

    public DbSet<GameManagementUnit> GameManagementUnits => Set<GameManagementUnit>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(
                    "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EnvWildlifeMortalities;Integrated Security=True;",
                    options => options.EnableRetryOnFailure()
                )
                .UseEnumCheckConstraints()
                .EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().ToTable("People");
        modelBuilder.Entity<Client>().HasIndex(c => c.EnvClientId).IsUnique();
        modelBuilder.Entity<ConservationOfficer>().HasIndex(c => c.BadgeNumber).IsUnique();

        modelBuilder.Entity<Seal>(s =>
        {
            s.ToTable("Seals");
            s.Property(s => s.Species).HasConversion<string>();
        });

        modelBuilder.Entity<Mortality>(m =>
        {
            m.ToTable("Mortalities");
            m.Property(m => m.Sex).HasConversion<string>();
        });
        modelBuilder
            .Entity<MortalityReport>()
            .HasOne(m => m.Mortality)
            .WithOne(m => m.MortalityReport);
        modelBuilder.Entity<HuntedHarvestReport>(h =>
        {
            h.HasOne(t => t.Mortality).WithOne();
            h.Property(h => h.Status).HasConversion<string>();
        });

        modelBuilder
            .Entity<GameManagementArea>()
            .Property(a => a.Area)
            .HasComputedColumnSql("[Zone] + [Subzone]", true);
        modelBuilder
            .Entity<GameManagementAreaSpecies>()
            .Property(s => s.Species)
            .HasConversion<string>()
            .HasMaxLength(25);
        modelBuilder.Entity<GameManagementAreaSchedule>(s =>
        {
            s.Property(s => s.PeriodStart).HasColumnType("date");
            s.Property(s => s.PeriodEnd).HasColumnType("date");
        });
        modelBuilder.Entity<GameManagementUnit>(u =>
        {
            u.Property(u => u.ActiveFrom).HasColumnType("date");
            u.Property(u => u.ActiveTo).HasColumnType("date");
            u.Property(u => u.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<BioSubmission>().OwnsOne(b => b.Age, a => a.ToTable("Age"));

        // These shadow properties are referenced during ETL to sync licence and seal data from their source (POSSE)
        modelBuilder.Entity<Authorization>().Property<int?>("PosseId");
        modelBuilder.Entity<Seal>().Property<int?>("PosseId");

        base.OnModelCreating(modelBuilder);
    }
}
