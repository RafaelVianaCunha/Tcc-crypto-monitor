using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CryptoMonitor.Clients;
using Microsoft.Azure.ServiceBus;
using CryptoMonitor.Entities;
using CryptoMonitor.Messages;
using System.Text;

namespace CryptoMonitor.Services
{
    public class StopLimitProcessor
    {
        public IReadOnlyCollection<IExchangeClient> ExchangeClients { get; }

        public PriceCalculator PriceCalculator { get; }

        public decimal StopLimit => 9500M;

        public IQueueClient QueueClient { get; }

        public StopLimitProcessor(
            IReadOnlyCollection<IExchangeClient> exchangeClients, 
            PriceCalculator priceCalculator, 
            IQueueClient queueClient)
        {
            ExchangeClients = exchangeClients;
            PriceCalculator = priceCalculator;
            QueueClient = queueClient;
        }

        public Task Process()
        {
            var consumers = ExchangeClients.Select(c =>
                c.ConsumeCoinValue("BTC-USD", async (coin) =>
                {
                    Console.WriteLine(JsonConvert.SerializeObject(coin));

                    var median = PriceCalculator.CalculateMedianForNewCoinValue(coin.Amount, coin.Exchange);

                    if (StopLimit >= median)
                    {
                        await SendNewSalesOrder(coin);
                    }
                })
            );

            return Task.WhenAll(consumers);
        }

        public async Task SendNewSalesOrder(Coin coin)
        {
            try
            {
                var newSalesOrder = new NewSalesOrder 
                {
                    UserId = Guid.NewGuid(),
                    Coin = coin,
                    Stop = 7000,
                    Limit = 8500,
                    Quantity = 1
                };

                var newSalesOrderMessageBody = JsonConvert.SerializeObject(newSalesOrder);
                Console.WriteLine(newSalesOrderMessageBody);
                var newSalesOrderMessage = new Message(Encoding.UTF8.GetBytes(newSalesOrderMessageBody));
                
                await QueueClient.SendAsync(newSalesOrderMessage);
                await QueueClient.CloseAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}