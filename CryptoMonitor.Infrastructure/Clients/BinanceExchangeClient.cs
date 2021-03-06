using System;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net;
using CryptoMonitor.Domain.Interfaces;
using CryptoMonitor.Domain.ValueObjects;
using CryptoMonitor.Enums;

namespace CryptoMonitor.Infraestructure.Clients
{
    public class BinanceExchangeClient : IExchangeClient
    {
        public BinanceSocketClient BinanceSocketClient { get; }

        public Exchanges Exchange => Exchanges.Binance;

        public BinanceExchangeClient(BinanceSocketClient binanceSocketClient)
        {
            BinanceSocketClient = binanceSocketClient;
        }

        public Task ConsumeOrderBook(string symbol, Action<OrderBook> handler, CancellationToken token)
        {
            var binanceSymbol = "BTCUSDT";

            return Task.Run(() =>
            {
                try
                {
                    BinanceSocketClient.SubscribeToSymbolTickerUpdatesAsync(binanceSymbol, (data) =>
                    {
                        handler(new OrderBook
                        {
                            Symbol = symbol,
                            Main = binanceSymbol,
                            Amount = data.BestBidPrice,
                            Exchange = Exchange
                        });
                    });
                }
                catch (System.Exception)
                {

                }
            });
        }

        public Task Unsubscribe()
        {
            Console.WriteLine("unsubscribing binance");
            return BinanceSocketClient.UnsubscribeAll();
        }
    }

    
}