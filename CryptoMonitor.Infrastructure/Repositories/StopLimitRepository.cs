using System.Threading.Tasks;
using CryptoMonitor.Domain.Entities;
using CryptoMonitor.Domain.Repositories;

namespace CryptoMonitor.Infraestructure.Repositories
{
    public class StopLimitRepository : IStopLimitRepository
    {
        public Task<StopLimit> Get()
        {
            throw new System.NotImplementedException();
        }
    }
}