using System;
using System.Linq;
using System.Threading.Tasks;
using Bittrex.Net;
using CryptoMonitor.Entities;
using CryptoMonitor.Enums;

namespace CryptoMonitor.Clients
{
    public class BittrexExchangeClient : IExchangeClient
    {
        public BittrexSocketClient BittrexSocketClient { get; }

        public Exchange Exchange => Exchange.Bittrex;

        public BittrexExchangeClient(BittrexSocketClient bittrexSocketClient)
        {
            BittrexSocketClient = bittrexSocketClient;
        }

        public Task ConsumeCoinValue(string symbol, Action<Coin> onNewValue)
        {
            var bittexSymbol = "USD-BTC";

            return BittrexSocketClient.SubscribeToOrderBookUpdatesAsync(bittexSymbol, (data) =>
            {
                var lastPrice = data.Buys
                    .Select(s => s.Price)
                    .OrderByDescending(x => x)
                    .First();

                onNewValue(new Coin
                {
                    Name = symbol,
                    Main = bittexSymbol,
                    Amount = lastPrice,
                    Exchange = Exchange
                });
            });
        }
    }
}