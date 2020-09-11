using System.Threading.Tasks;
using CryptoMonitor.Domain.Entities;

namespace CryptoMonitor.Domain.Services.Interfaces
{
    public interface IStopLimitOrderService
    {
        void Monitor(StopLimit stopLimit);
    }
}