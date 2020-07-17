using System;
using System.Linq;
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

        public Task ConsumeOrderBook(string symbol, Action<OrderBook> onNewValue)
        {
            var bittexSymbol = "USD-BTC";

            return BittrexSocketClient.SubscribeToOrderBookUpdatesAsync(bittexSymbol, (data) =>
            {
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
            });
        }
    }
}