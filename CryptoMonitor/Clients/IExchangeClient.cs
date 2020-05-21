using System;
using System.Threading.Tasks;
using CryptoMonitor.Entities;
using CryptoMonitor.Enums;

namespace CryptoMonitor.Clients {
    public interface IExchangeClient
    {
        Exchange Exchange { get; }

        Task ConsumeCoinValue(string symbol, Action<Coin> onNewValue);
    }
}