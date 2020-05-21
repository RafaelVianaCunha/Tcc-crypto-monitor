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

        public PriceCalculator PriceCalculator { get; }

        public decimal StopLimit => 9500M;

        public StopLimitProcessor(IReadOnlyCollection<IExchangeClient> exchangeClients, PriceCalculator priceCalculator)
        {
            ExchangeClients = exchangeClients;
            PriceCalculator = priceCalculator;
        }

        public Task Process()
        {
            var consumers = ExchangeClients.Select(c =>
            
                c.ConsumeCoinValue("BTC-USD", (coin) =>
                {
                    Console.WriteLine(JsonConvert.SerializeObject(coin));

                    var median = PriceCalculator.CalculateMedianForNewCoinValue(coin.Amount, coin.Exchange);

                    if (StopLimit >= median)
                    {
                        Console.WriteLine("Tem que vender");
                    }
                })
            );

            return Task.WhenAll(consumers);
        }
    }
}