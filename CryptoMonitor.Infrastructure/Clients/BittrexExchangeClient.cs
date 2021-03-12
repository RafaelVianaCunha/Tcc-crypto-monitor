using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net;
using CryptoMonitor.Domain.Interfaces;
using CryptoMonitor.Domain.ValueObjects;
using CryptoMonitor.Enums;

namespace CryptoMonitor.Infraestructure.Clients
{
    public class BittrexExchangeClient : IExchangeClient
    {
        public BittrexSocketClient BittrexSocketClient { get; }

        public Exchanges Exchange => Exchanges.Bittrex;

        public BittrexExchangeClient(BittrexSocketClient bittrexSocketClient)
        {
            BittrexSocketClient = bittrexSocketClient;
        }

        public Task ConsumeOrderBook(string symbol, Action<OrderBook> onNewValue, CancellationToken token)
        {
            var bittexSymbol = "USD-BTC";

            return Task.Run(() => BittrexSocketClient.SubscribeToOrderBookUpdatesAsync(bittexSymbol, (data) =>
            {
                try {
                    var lastPrice = data.Buys
                    .Select(s => s.Price)
                    .OrderByDescending(x => x)
                    .First();

                onNewValue(new OrderBook
                 {
                    Symbol = symbol,
                    Main = bittexSymbol,
                    Amount = lastPrice,
                    Exchange = Exchange
                });
                } catch (Exception) {
                    
                }
            }));
        }

        public Task Unsubscribe()
        {
            Console.WriteLine("unsubscribing bittrex");
            return BittrexSocketClient.UnsubscribeAll();
        }
    }
}