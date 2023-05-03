using AppCore.Contracts.Common;
using AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class MainDbContext: AppDbContextBase
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DTTariffMain> Tariffs { get; set; }
        public DbSet<DTCostingMain> DTCostings { get; set; }
        public DbSet<DTCostingCapacity> DTCapacities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: define model later
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
        }
    }
}
