using System;
using System.Threading.Tasks;
using Binance.Net;
using CryptoMonitor.Entities;
using CryptoMonitor.Enums;

namespace CryptoMonitor.Clients
{
    public class BinanceExchangeClient : IExchangeClient
    {
        public BinanceSocketClient BinanceSocketClient { get; }

        public Exchange Exchange => Exchange.Binance;

        public BinanceExchangeClient(BinanceSocketClient binanceSocketClient)
        {
            BinanceSocketClient = binanceSocketClient;
        }

        public Task ConsumeCoinValue(string symbol, Action<Coin> handler)
        {
            var binanceSymbol = "BTCUSDT";

            return BinanceSocketClient.SubscribeToSymbolTickerUpdatesAsync(binanceSymbol, (data) =>
            {
                handler(new Coin
                {
                    Name = symbol,
                    Main = binanceSymbol,
                    Amount = data.BestBidPrice,
                    Exchange = Exchange
                });
            });
        }
    }
}