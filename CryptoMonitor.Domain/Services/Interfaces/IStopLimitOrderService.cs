using System.Threading.Tasks;

namespace CryptoMonitor.Domain.Services.Interfaces
{
    public interface IStopLimitOrderService
    {
        Task MonitorExchanges();
    }
}