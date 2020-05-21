using System;
using System.Threading.Tasks;
using Bitfinex.Net;
using CryptoMonitor.Entities;
using CryptoMonitor.Enums;

namespace CryptoMonitor.Clients
{
    public class BitfinexExchangeClient : IExchangeClient
    {
        public Exchange Exchange => Exchange.Bitfinex;

        public BitfinexSocketClient BitfinexSocketClient { get; }

        public BitfinexExchangeClient(BitfinexSocketClient bitfinexSocketClient)
        {
            BitfinexSocketClient = bitfinexSocketClient;
        }

        public Task ConsumeCoinValue(string symbol, Action<Coin> onNewValue)
        {
            var bitfinexSymbol = "tBTCUSD";

            return BitfinexSocketClient.SubscribeToTickerUpdatesAsync(bitfinexSymbol, (data) =>
            {
                onNewValue(new Coin
                {
                    Name = symbol,
                    Main = bitfinexSymbol,
                    Amount = data.LastPrice,
                    Exchange = Exchange
                });
            });
        }
    }
}