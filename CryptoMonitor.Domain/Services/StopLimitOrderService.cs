using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        private CancellationTokenSource _tokenSource;

        public StopLimitOrderService(IReadOnlyCollection<IExchangeClient> exchangeClients, IPriceCalculator priceCalculator, IStopLimitRepository stopLimitRepository, IOrderSaleQueueClient orderSaleQueueClient)
        {
            ExchangeClients = exchangeClients;
            PriceCalculator = priceCalculator;
            StopLimitRepository = stopLimitRepository;
            OrderSaleQueueClient = orderSaleQueueClient;

            var tokenSource = new CancellationTokenSource();
            _tokenSource = tokenSource;
        }

        public async Task Monitor(StopLimit stopLimit)
        {
            var consumers = ExchangeClients.Select(c =>
                c.ConsumeOrderBook("BTC-USD", async (orderBook) =>
                {
                    var median = PriceCalculator.CalculateMedianForNewCoinValue(orderBook.Amount, orderBook.Exchange);

                    if (stopLimit.Stop >= median)
                    {
                        await SendNewOrderSale(stopLimit);
                    }
                }, _tokenSource.Token)
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

            _tokenSource.Cancel();
        }
    }
}   