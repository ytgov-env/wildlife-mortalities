using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.GuidedReports;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.MortalityReports;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Person> People => Set<Person>();

    public DbSet<Authorization> Authorizations => Set<Authorization>();
    public DbSet<Seal> Seals => Set<Seal>();

    public DbSet<Mortality> Mortalities => Set<Mortality>();
    public DbSet<MortalityReport> MortalityReports => Set<MortalityReport>();
    public DbSet<Violation> Violations => Set<Violation>();

    public DbSet<OutfitterGuidedHuntReport> OutfitterGuidedHuntReports =>
        Set<OutfitterGuidedHuntReport>();

    public DbSet<SpecialGuidedHuntReport> SpecialGuidedHuntReports =>
        Set<SpecialGuidedHuntReport>();

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
        modelBuilder.Entity<AmericanBlackBearMortality>();
        modelBuilder.Entity<AmericanBlackBearBioSubmission>();

        modelBuilder.ApplyConfiguration(new PersonConfig());
        modelBuilder.ApplyConfiguration(new ClientConfig());
        modelBuilder.ApplyConfiguration(new ConservationOfficerConfig());

        modelBuilder.ApplyConfiguration(new MortalityConfig());

        modelBuilder.ApplyConfiguration(new MortalityReportConfig());
        modelBuilder.ApplyConfiguration(new HuntedMortalityReportConfig());
        modelBuilder.ApplyConfiguration(new HumanWildlifeConflictMortalityReportConfig());

        modelBuilder.ApplyConfiguration(new GameManagementAreaConfig());
        modelBuilder.ApplyConfiguration(new GameManagementAreaSpeciesConfig());
        modelBuilder.ApplyConfiguration(new GameManagementAreaScheduleConfig());
        modelBuilder.ApplyConfiguration(new GameManagementUnitConfig());

        modelBuilder.ApplyConfiguration(new BioSubmissionConfig());

        modelBuilder.ApplyConfiguration(new OutfitterAreaConfig());

        modelBuilder.ApplyConfiguration(new AuthorizationConfig());
        modelBuilder.ApplyConfiguration(new SealConfig());

        base.OnModelCreating(modelBuilder);
    }
}
