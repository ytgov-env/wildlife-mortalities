﻿using Microsoft.EntityFrameworkCore;
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
        //public DbSet<Mortality> Mortalities => Set<Mortality>();

        public DbSet<HuntingMortality> HuntingMortalities => Set<HuntingMortality>();
        public DbSet<TrappingMortality> TrappingMortalities => Set<TrappingMortality>();
        public DbSet<BirdMortality> BirdMortalities => Set<BirdMortality>();
        public DbSet<HarvestReport> HarvestReports => Set<HarvestReport>();
        public DbSet<BiologicalSubmission> BiologicalSubmissions => Set<BiologicalSubmission>();
        //public DbSet<Bird> Birds => Set<Bird>();
        //public DbSet<Bison> Bisons => Set<Bison>();
        //public DbSet<BlackBear> BlackBears => Set<BlackBear>();
        //public DbSet<Caribou> Caribous => Set<Caribou>();
        //public DbSet<Coyote> Coyotes => Set<Coyote>();
        //public DbSet<Deer> Deers => Set<Deer>();
        //public DbSet<Elk> Elks => Set<Elk>();
        //public DbSet<Goat> Goats => Set<Goat>();
        //public DbSet<GrizzlyBear> GrizzlyBears => Set<GrizzlyBear>();
        //public DbSet<Moose> Moose => Set<Moose>();
        //public DbSet<Sheep> Sheep => Set<Sheep>();
        //public DbSet<Wolf> Wolves => Set<Wolf>();
        //public DbSet<Wolverine> Wolverines => Set<Wolverine>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EnvWildlifeMortalities;Integrated Security=True;")
                .UseEnumCheckConstraints();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MortalityBase>().ToTable("Mortalities");

            modelBuilder.Entity<MortalityBase>().Property(m => m.Species).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<TrappingMortality>().Property(t => t.Sex).HasConversion<string>().HasMaxLength(25);
            modelBuilder.Entity<HuntingMortality>().Property(h => h.Sex).HasConversion<string>().HasMaxLength(25);

            modelBuilder.Entity<TrappingMortality>().Property(t => t.Sex).HasColumnName("Sex");
            modelBuilder.Entity<HuntingMortality>().Property(h => h.Sex).HasColumnName("Sex");

            modelBuilder.Entity<TrappingMortality>().Property(t => t.SealNumber).HasColumnName("SealNumber");
            modelBuilder.Entity<HuntingMortality>().Property(h => h.SealNumber).HasColumnName("SealNumber");

            modelBuilder.Entity<BirdMortality>().Property(b => b.Quantity).HasColumnName("Quantity");
            modelBuilder.Entity<TrappingMortality>().Property(t => t.Quantity).HasColumnName("Quantity");

            //modelBuilder.Entity<Bird>().ToTable("Birds");
            //modelBuilder.Entity<Bison>().ToTable("Bisons");
            //modelBuilder.Entity<BlackBear>().ToTable("BlackBears");
            //modelBuilder.Entity<Caribou>().ToTable("Caribou");
            //modelBuilder.Entity<Coyote>().ToTable("Coyotes");
            //modelBuilder.Entity<Deer>().ToTable("Deers");
            //modelBuilder.Entity<Elk>().ToTable("Elks");
            //modelBuilder.Entity<Goat>().ToTable("Goats");
            //modelBuilder.Entity<GrizzlyBear>().ToTable("GrizzlyBears");
            //modelBuilder.Entity<Moose>().ToTable("Moose");
            //modelBuilder.Entity<Sheep>().ToTable("Sheep");
            //modelBuilder.Entity<Wolf>().ToTable("Wolves");
            //modelBuilder.Entity<Wolverine>().ToTable("Wolverines");
            base.OnModelCreating(modelBuilder);
        }
    }
}
