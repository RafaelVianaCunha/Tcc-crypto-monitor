using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoMonitor.Domain.Entities;

namespace CryptoMonitor.Domain.Repositories
{
    public interface IStopLimitRepository
    {
        Task<StopLimit> Delete(StopLimit stopLimit);

        Task<IEnumerable<StopLimit>> Get();
    }
}