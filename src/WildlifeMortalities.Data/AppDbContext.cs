﻿using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Entities.Users;

// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace WildlifeMortalities.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<Person> People => Set<Person>();
    public DbSet<OutfitterGuide> OutfitterGuides => Set<OutfitterGuide>();
    public DbSet<Season> Seasons => Set<Season>();

    public DbSet<Authorization> Authorizations => Set<Authorization>();
    public DbSet<InvalidAuthorization> InvalidAuthorizations => Set<InvalidAuthorization>();
    public DbSet<Mortality> Mortalities => Set<Mortality>();
    public DbSet<Entities.Reports.SingleMortality.Activity> Activities =>
        Set<Entities.Reports.SingleMortality.Activity>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<DraftReport> DraftReports => Set<DraftReport>();
    public DbSet<DeletedReport> DeletedReports => Set<DeletedReport>();
    public DbSet<Violation> Violations => Set<Violation>();
    public DbSet<BioSubmission> BioSubmissions => Set<BioSubmission>();

    public DbSet<GameManagementArea> GameManagementAreas => Set<GameManagementArea>();
    public DbSet<GameManagementSubArea> GameManagementSubAreas => Set<GameManagementSubArea>();
    public DbSet<OutfitterArea> OutfitterAreas => Set<OutfitterArea>();

    public DbSet<RegisteredTrappingConcession> RegisteredTrappingConcessions =>
        Set<RegisteredTrappingConcession>();

    public DbSet<BagEntry> BagEntries => Set<BagEntry>();
    public DbSet<BagLimitEntry> BagLimitEntries => Set<BagLimitEntry>();
    public DbSet<ActivityQueueItem> ActivityQueueItems => Set<ActivityQueueItem>();

    public DbSet<AppConfiguration> AppConfigurations => Set<AppConfiguration>();

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
        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(1000)
            .HaveConversion<TrimConverter>();
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
