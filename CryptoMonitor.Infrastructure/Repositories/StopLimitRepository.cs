using System.Linq;
using System.Threading.Tasks;
using CryptoMonitor.Domain.Entities;
using CryptoMonitor.Domain.Repositories;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CryptoMonitor.Infraestructure.Repositories
{
    public class StopLimitRepository : IStopLimitRepository
    {
        public CryptoMonitorDbContext DbContext { get; }

        public StopLimitRepository(CryptoMonitorDbContext dbContext) 
        {   
            DbContext = dbContext;
        }
        public async Task<StopLimit> Delete(StopLimit stopLimit)
        {
            stopLimit.DeletedAt = DateTime.UtcNow;

            var local = DbContext.Set<StopLimit>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(stopLimit.Id));

            if (local != null)
            {
                DbContext.Entry(local).State = EntityState.Detached;
            }

            DbContext.StopLimits.Attach(stopLimit);
            DbContext.Entry(stopLimit).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return stopLimit;
        }

        public async Task<IEnumerable<StopLimit>> Get()
        {
            return await DbContext.StopLimits.Where(x => x.DeletedAt == null).ToListAsync();
        }
    }
}