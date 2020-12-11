using System;
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
using Newtonsoft.Json;

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

        public void Monitor(StopLimit stopLimit)
        {
            try
            {
                foreach (var exchangeClient in ExchangeClients)
                {
                    exchangeClient.ConsumeOrderBook("BTC-USD", async (orderBook) =>
                    {
                        var median = PriceCalculator.CalculateMedianForNewCoinValue(orderBook.Amount, orderBook.Exchange);

                        Console.WriteLine(exchangeClient.Exchange + " : " + JsonConvert.SerializeObject(orderBook) + " : Mediana" + median);

                        if (stopLimit.Stop >= median && !_tokenSource.IsCancellationRequested)
                        {
                            Console.WriteLine("Ordem Stop Loss acionada");
                            //await SendNewOrderSale(stopLimit);

                            foreach (var t in ExchangeClients)
                            {
                                await t.Unsubscribe();
                            }

                            _tokenSource.Cancel();
                        }
                    }, _tokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                var e = ex;
            }
        }

        private async Task SendNewOrderSale(StopLimit stopLimit)
        {
            var orderSale = new OrderSale
            {
                StopLimitId = stopLimit.Id,
                StopLimit = stopLimit
            };

            try
            {
                await OrderSaleQueueClient.Queue(orderSale);
            }
            catch (Exception ex)
            {
                var t = ex;
            }


            _tokenSource.Cancel();
        }
    }
}