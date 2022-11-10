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
    public DbSet<OutfitterArea> OutfitterAreas => Set<OutfitterArea>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(
                    "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=EnvWildlifeMortalities;Integrated Security=True;",
                    options => options.EnableRetryOnFailure()
                )
                .UseEnumCheckConstraints()
                .EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureMortalities(modelBuilder);

        modelBuilder.Entity<AmericanBlackBearBioSubmission>();

        modelBuilder.ApplyConfiguration(new PersonConfig());
        modelBuilder.ApplyConfiguration(new ClientConfig());
        modelBuilder.ApplyConfiguration(new ConservationOfficerConfig());

        modelBuilder.ApplyConfiguration(new MortalityConfig<Mortality>());

        modelBuilder.ApplyConfiguration(new MortalityReportConfig());
        modelBuilder.ApplyConfiguration(new HuntedMortalityReportConfig());
        modelBuilder.ApplyConfiguration(new HumanWildlifeConflictMortalityReportConfig());

        modelBuilder.ApplyConfiguration(new GameManagementAreaConfig());
        modelBuilder.ApplyConfiguration(new OutfitterAreaConfig());

        modelBuilder.ApplyConfiguration(new BioSubmissionConfig());

        modelBuilder.ApplyConfiguration(new AuthorizationConfig());
        modelBuilder.ApplyConfiguration(new SpecialGuideLicenceConfig());
        modelBuilder.ApplyConfiguration(new HuntingLicenceConfig());
        modelBuilder.ApplyConfiguration(new HuntingPermitConfig());
        modelBuilder.ApplyConfiguration(new PermitHuntAuthorizationConfig());
        modelBuilder.ApplyConfiguration(new SealConfig());

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureMortalities(ModelBuilder modelBuilder)
    {
        var mortalityType = typeof(Mortality);
        var relevantAssembly = mortalityType.Assembly;
        var allTypes = relevantAssembly.GetTypes();

        foreach (var item in allTypes)
        {
            if (item.IsSubclassOf(mortalityType) != true)
            {
                continue;
            }

            var constructor = item.GetConstructor(Array.Empty<Type>());
            if (constructor == null)
            {
                continue;
            }

            modelBuilder.Entity(item);

            var instance = constructor.Invoke(Array.Empty<object>());

            var configMethod = item.GetMethod(nameof(Mortality<Mortality>.GetConfig));
            if (configMethod == null)
            {
                continue;
            }

            configMethod.Invoke(instance, Array.Empty<object>());
        }
    }
}
