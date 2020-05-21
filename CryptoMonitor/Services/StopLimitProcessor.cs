using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CryptoMonitor.Clients;

namespace CryptoMonitor.Services
{
    public class StopLimitProcessor
    {
        public IReadOnlyCollection<IExchangeClient> ExchangeClients { get; }

        public StopLimitProcessor(IReadOnlyCollection<IExchangeClient> exchangeClients)
        {
            ExchangeClients = exchangeClients;
        }

        public Task Process()
        {
            var consumers = ExchangeClients.Select(c =>
            
                c.ConsumeCoinValue("BTC-USD", (coin) =>
                {
                    Console.WriteLine(JsonConvert.SerializeObject(coin));
                })
            );

            return Task.WhenAll(consumers);
        }
    }
}