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
        public DbSet<Bison> Bisons => Set<Bison>();
        public DbSet<BlackBear> BlackBears => Set<BlackBear>();
        public DbSet<Caribou> Caribous => Set<Caribou>();
        public DbSet<Coyote> Coyotes => Set<Coyote>();
        public DbSet<Deer> Deers => Set<Deer>();
        public DbSet<Elk> Elks => Set<Elk>();
        public DbSet<Goat> Goats => Set<Goat>();
        public DbSet<GrizzlyBear> GrizzlyBears => Set<GrizzlyBear>();
        public DbSet<Moose> Moose => Set<Moose>();
        public DbSet<Sheep> Sheep => Set<Sheep>();
        public DbSet<Wolf> Wolves => Set<Wolf>();
        public DbSet<Wolverine> Wolverines => Set<Wolverine>();
    }
}
