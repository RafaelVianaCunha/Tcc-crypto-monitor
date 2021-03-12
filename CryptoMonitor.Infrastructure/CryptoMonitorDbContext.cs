using System;
using CryptoMonitor.Domain.Entities;
using CryptoMonitor.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitor.Infraestructure
{
    public class CryptoMonitorDbContext : DbContext, IDisposable
    {
        public DbSet<StopLimit> StopLimits { get; set; }

        public CryptoMonitorDbContext(){
            
        }

        public CryptoMonitorDbContext(DbContextOptions options) : base(options)
        {
        }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StopLimitConfiguration());
        }
    
    }

}