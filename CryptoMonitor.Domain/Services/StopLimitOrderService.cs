using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoMonitor.Domain.Clients;
using CryptoMonitor.Domain.Entities;
using CryptoMonitor.Domain.Interfaces;
using CryptoMonitor.Domain.Repositories;
using CryptoMonitor.Domain.Services.Interfaces;
using CryptoMonitor.Domain.ValueObjects;

namespace CryptoMonitor.Domain.Services
{
    public class StopLimitOrderService : IStopLimitOrderService
    {
        public IReadOnlyCollection<IExchangeClient> ExchangeClients { get; }

        public IPriceCalculator PriceCalculator { get; }

        public IStopLimitRepository StopLimitRepository { get; }

        public IOrderSaleQueueClient OrderSaleQueueClient { get; }

        public StopLimitOrderService(IReadOnlyCollection<IExchangeClient> exchangeClients, IPriceCalculator priceCalculator, IStopLimitRepository stopLimitRepository, IOrderSaleQueueClient orderSaleQueueClient)
        {
            ExchangeClients = exchangeClients;
            PriceCalculator = priceCalculator;
            StopLimitRepository = stopLimitRepository;
            OrderSaleQueueClient = orderSaleQueueClient;
        }

        public async Task MonitorExchanges()
        {
            var stopLimit = await StopLimitRepository.Get();   

            var consumers = ExchangeClients.Select(c =>
                c.ConsumeOrderBook("BTC-USD", async (orderBook) =>
                {
                    var median = PriceCalculator.CalculateMedianForNewCoinValue(orderBook.Amount, orderBook.Exchange);

                    if (stopLimit.Stop >= median)
                    {
                        await SendNewOrderSale(stopLimit);
                    }
                })
            );         
        }

        private async Task SendNewOrderSale(StopLimit stopLimit)
        {
            var orderSale = new OrderSale
            {
                StopLimitId = stopLimit.Id,
                StopLimit = stopLimit
            };

            await OrderSaleQueueClient.Queue(orderSale);
        }
    }
}   