using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoMonitor.Domain.ValueObjects;
using CryptoMonitor.Enums;

namespace CryptoMonitor.Domain.Interfaces
{
    public interface IExchangeClient
    {
        Exchanges Exchange { get; }
        Task ConsumeOrderBook(string symbol, Action<OrderBook> action, CancellationToken token);
    }
}