using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;

// ReSharper disable ReturnTypeCanBeEnumerable.Global

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
    public DbSet<Mortality> Mortalities => Set<Mortality>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<Violation> Violations => Set<Violation>();

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
        modelBuilder.Entity<AmericanBlackBearBioSubmission>();

        ConfigureMortalities(modelBuilder);
        modelBuilder.ApplyConfiguration(new MortalityConfig<Mortality>());

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureMortalities(ModelBuilder modelBuilder)
    {
        var mortalityType = typeof(Mortality);
        var relevantAssembly = mortalityType.Assembly;
        var allTypes = relevantAssembly.GetTypes();

        foreach (var item in allTypes)
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
