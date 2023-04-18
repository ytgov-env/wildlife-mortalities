using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Users;

// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace WildlifeMortalities.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<Season> Seasons => Set<Season>();

    public DbSet<Authorization> Authorizations => Set<Authorization>();
    public DbSet<Mortality> Mortalities => Set<Mortality>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<DraftReport> DraftReports => Set<DraftReport>();
    public DbSet<Violation> Violations => Set<Violation>();

    public DbSet<BioSubmission> BioSubmissions => Set<BioSubmission>();

    public DbSet<FurbearerSealingCertificate> FurbearerSealingCertificates =>
        Set<FurbearerSealingCertificate>();

    public DbSet<GameManagementArea> GameManagementAreas => Set<GameManagementArea>();
    public DbSet<OutfitterArea> OutfitterAreas => Set<OutfitterArea>();

    public DbSet<RegisteredTrappingConcession> RegisteredTrappingConcessions =>
        Set<RegisteredTrappingConcession>();

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

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(1000);
        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureMortalities(modelBuilder);
        modelBuilder.ApplyConfiguration(new MortalityConfig<Mortality>());

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureMortalities(ModelBuilder modelBuilder)
    {
        var mortalityType = typeof(Mortality);
        var relevantAssembly = mortalityType.Assembly;
        foreach (var item in relevantAssembly.GetTypes())
        {
            if (!item.IsSubclassOf(mortalityType))
            {
                continue;
            }

            var constructor = item.GetConstructor(Array.Empty<Type>());
            if (constructor == null)
            {
                continue;
            }

            modelBuilder.Entity(item);
        }
    }
}
