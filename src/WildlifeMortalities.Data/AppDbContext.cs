using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Licences;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reporters;

namespace WildlifeMortalities.Data;

public class AppDbContext : DbContext
{
    public DbSet<Reporter> Reporters => Set<Reporter>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ConservationOfficer> ConservationOfficers => Set<ConservationOfficer>();

    public DbSet<Licence> Licences => Set<Licence>();
    public DbSet<Seal> Seals => Set<Seal>();

    public DbSet<Mortality> Mortalities => Set<Mortality>();
    public DbSet<BisonMortality> BisonMortalities => Set<BisonMortality>();
    public DbSet<BlackBearMortality> BlackBearMortalities => Set<BlackBearMortality>();



    public DbSet<HarvestReport> HarvestReports => Set<HarvestReport>();
    public DbSet<TrappedHarvestReport> TrappedHarvestReports => Set<TrappedHarvestReport>();
    public DbSet<HuntedHarvestReport> HuntedHarvestReports => Set<HuntedHarvestReport>();

    public DbSet<BioSubmission> BioSubmissions => Set<BioSubmission>();

    public DbSet<GameManagementArea> GameManagementAreas => Set<GameManagementArea>();
    public DbSet<GameManagementAreaSpecies> GameManagementAreaSpecies => Set<GameManagementAreaSpecies>();
    public DbSet<GameManagementAreaSchedule> GameManagementAreaSchedules => Set<GameManagementAreaSchedule>();
    public DbSet<GameManagementUnit> GameManagementUnits => Set<GameManagementUnit>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EnvWildlifeMortalities;Integrated Security=True;",
            x => x.UseNetTopologySuite())
            .UseEnumCheckConstraints();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Seal>().ToTable("Seals");
        modelBuilder.Entity<Mortality>().ToTable("Mortalities");

        modelBuilder.Entity<Seal>().Property(s => s.Species).HasConversion<string>();

        modelBuilder.Entity<TrappedHarvestReport>().HasMany(t => t.Mortalities).WithOne();
        modelBuilder.Entity<HuntedHarvestReport>().HasOne(t => t.Mortality).WithOne();

        modelBuilder.Entity<Mortality>().HasOne(m => m.Reporter)
            .WithMany(r => r.Mortalities).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<GameManagementAreaSpecies>().Property(s => s.Species).HasConversion<string>().HasMaxLength(25);

        modelBuilder.Entity<GameManagementArea>().Property(a => a.Area).HasComputedColumnSql("[Zone] + [Subzone]", true);

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
