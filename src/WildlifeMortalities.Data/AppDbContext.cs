using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Entities;

namespace WildlifeMortalities.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Licence> Licences => Set<Licence>();
        public DbSet<Seal> Seals => Set<Seal>();
        public DbSet<HuntedMortality> HuntedMortalities => Set<HuntedMortality>();
        public DbSet<TrappedMortality> TrappingMortalities => Set<TrappedMortality>();
        public DbSet<BirdMortality> BirdMortalities => Set<BirdMortality>();
        public DbSet<HarvestReport> HarvestReports => Set<HarvestReport>();
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

            modelBuilder.Entity<MortalityBase>().Property(m => m.Species).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<TrappedMortality>().Property(t => t.Sex).HasConversion<string>().HasMaxLength(25);
            modelBuilder.Entity<HuntedMortality>().Property(h => h.Sex).HasConversion<string>().HasMaxLength(25);
            modelBuilder.Entity<Licence>().Property(l => l.Type).HasConversion<string>().HasMaxLength(25);
            modelBuilder.Entity<GameManagementAreaSpecies>().Property(s => s.Species).HasConversion<string>().HasMaxLength(25);

            modelBuilder.Entity<TrappedMortality>().Property(t => t.Sex).HasColumnName("Sex");
            modelBuilder.Entity<HuntedMortality>().Property(h => h.Sex).HasColumnName("Sex");

            modelBuilder.Entity<BirdMortality>().Property(b => b.Quantity).HasColumnName("Quantity");
            modelBuilder.Entity<TrappedMortality>().Property(t => t.Quantity).HasColumnName("Quantity");

            modelBuilder.Entity<GameManagementArea>().Property(a => a.ZoneSubzone).HasComputedColumnSql("[Zone] * 100 + [Subzone]", true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
