using System;
using System.Threading.Tasks;
using Bitfinex.Net;
using CryptoMonitor.Domain.Interfaces;
using CryptoMonitor.Domain.ValueObjects;
using CryptoMonitor.Enums;

namespace CryptoMonitor.Infraestructure.Clients
{
    public class BitfinexExchangeClient : IExchangeClient
    {
        public Exchanges Exchange => Exchanges.Bitfinex;

        public BitfinexSocketClient BitfinexSocketClient { get; }

        public BitfinexExchangeClient(BitfinexSocketClient bitfinexSocketClient)
        {
            BitfinexSocketClient = bitfinexSocketClient;
        }

        public Task ConsumeOrderBook(string symbol, Action<OrderBook> onNewValue)
        {
            var bitfinexSymbol = "tBTCUSD";

            return BitfinexSocketClient.SubscribeToTickerUpdatesAsync(bitfinexSymbol, (data) =>
            {
                onNewValue(new OrderBook
                {
                    Symbol = symbol,
                    Main = bitfinexSymbol,
                    Amount = data.LastPrice,
                    Exchange = Exchange
                });
            });
        }
    }
}