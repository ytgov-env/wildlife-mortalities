using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;

namespace WildlifeMortalities.Data;

public class AppDbContext : DbContext
{
    public DbSet<Reporter> Reporters => Set<Reporter>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ConservationOfficer> ConservationOfficers => Set<ConservationOfficer>();
    public DbSet<Licence> Licences => Set<Licence>();
    public DbSet<Seal> Seals => Set<Seal>();
    public DbSet<HuntedMortality> HuntedMortalities => Set<HuntedMortality>();
    public DbSet<TrappedMortality> TrappingMortalities => Set<TrappedMortality>();
    public DbSet<BirdMortality> BirdMortalities => Set<BirdMortality>();
    public DbSet<HarvestReportBase> HarvestReports => Set<HarvestReportBase>();
    public DbSet<TrappedHarvestReport> TrappedHarvestReports => Set<TrappedHarvestReport>();
    public DbSet<HuntedHarvestReport> HuntedHarvestReports => Set<HuntedHarvestReport>();
    public DbSet<BiologicalSubmission> BiologicalSubmissions => Set<BiologicalSubmission>();
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
        modelBuilder.Entity<MortalityBase>().ToTable("Mortalities");

        modelBuilder.Entity<HuntedMortality>().HasOne(h => h.Seal)
            .WithMany(s => s.HuntedMortalities).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<TrappedMortality>().HasOne(t => t.Licence)
            .WithMany(s => s.TrappedMortalities).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<HuntedMortality>().HasOne(h => h.HarvestReport)
            .WithOne(h => h.Mortality).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<TrappedMortality>().HasOne(h => h.HarvestReport)
            .WithMany(h => h.Mortalities).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MortalityBase>().HasOne(m => m.Reporter)
            .WithMany(r => r.Mortalities).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MortalityBase>().Property(m => m.Species).HasConversion<string>().HasMaxLength(50);
        modelBuilder.Entity<TrappedMortality>().Property(t => t.Sex).HasConversion<string>().HasMaxLength(25);
        modelBuilder.Entity<HuntedMortality>().Property(h => h.Sex).HasConversion<string>().HasMaxLength(25);
        modelBuilder.Entity<Licence>().Property(l => l.Type).HasConversion<string>().HasMaxLength(25);
        modelBuilder.Entity<GameManagementAreaSpecies>().Property(s => s.Species).HasConversion<string>().HasMaxLength(25);

        modelBuilder.Entity<TrappedMortality>().Property(t => t.Sex).HasColumnName("Sex");
        modelBuilder.Entity<HuntedMortality>().Property(h => h.Sex).HasColumnName("Sex");

        modelBuilder.Entity<TrappedMortality>().Property(t => t.HarvestReportId).HasColumnName("HarvestReportId");
        modelBuilder.Entity<HuntedMortality>().Property(t => t.HarvestReportId).HasColumnName("HarvestReportId");

        modelBuilder.Entity<GameManagementArea>().Property(a => a.ZoneSubzone).HasComputedColumnSql("[Zone] * 100 + [Subzone]", true);

        modelBuilder.Entity<TrappedMortality>().Property(t => t.KillDate).HasColumnType("date");
        modelBuilder.Entity<GameManagementAreaSchedule>().Property(s => s.PeriodStart).HasColumnType("date");
        modelBuilder.Entity<GameManagementAreaSchedule>().Property(s => s.PeriodEnd).HasColumnType("date");
        modelBuilder.Entity<GameManagementUnit>().Property(u => u.ActiveFrom).HasColumnType("date");
        modelBuilder.Entity<GameManagementUnit>().Property(u => u.ActiveTo).HasColumnType("date");

        modelBuilder.Entity<HuntedMortality>().Property(h => h.Landmark).HasMaxLength(100);
        modelBuilder.Entity<TrappedMortality>().Property(t => t.KillType).HasMaxLength(50);
        modelBuilder.Entity<GameManagementUnit>().Property(u => u.Name).HasMaxLength(50);

        // These shadow properties are referenced during ETL to sync licence and seal data from their source (POSSE)
        modelBuilder.Entity<Licence>().Property<int?>("PosseId");
        modelBuilder.Entity<Seal>().Property<int?>("PosseId");

        base.OnModelCreating(modelBuilder);
    }
}
