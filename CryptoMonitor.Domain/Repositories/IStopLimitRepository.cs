using System.Threading.Tasks;
using CryptoMonitor.Domain.Entities;

namespace CryptoMonitor.Domain.Repositories
{
    public interface IStopLimitRepository
    {
        Task<StopLimit> Get();
    }
}