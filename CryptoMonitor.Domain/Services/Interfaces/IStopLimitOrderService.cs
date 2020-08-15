using System.Threading.Tasks;
using CryptoMonitor.Domain.Entities;

namespace CryptoMonitor.Domain.Services.Interfaces
{
    public interface IStopLimitOrderService
    {
        Task Monitor(StopLimit stopLimit);
    }
}