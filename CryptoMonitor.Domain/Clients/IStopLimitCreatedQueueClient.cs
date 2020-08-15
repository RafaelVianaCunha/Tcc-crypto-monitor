using System.Threading.Tasks;
using CryptoMonitor.Domain.ValueObjects;

namespace CryptoMonitor.Domain.Clients
{
    public interface IStopLimitCreatedQueueClient
    {
        void Consume();
    }
}