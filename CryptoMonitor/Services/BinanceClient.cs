using System;
using System.Threading.Tasks;
using Binance.Net;
using CryptoMonitor.Entities;

namespace CryptoMonitor
{
    public class BinanceClient
    {
        public BinanceSocketClient BinanceSocketClient { get; }

        public BinanceClient(BinanceSocketClient binanceSocketClient)
        {
            BinanceSocketClient = binanceSocketClient;
        }

        public Task ConsumeCoinValue(string symbol, Action<Coin> handler)
        {
            var binanceSymbol = symbol;
            using (var client = new BinanceSocketClient())
            {
                return client.SubscribeToSymbolTickerUpdatesAsync(binanceSymbol, (data) =>
                {
                    handler(new Coin {
                        Name = symbol,
                        Main = binanceSymbol,
                        Amount = data.BestBidPrice
                       
                    });
                });
            }
        }
    }
}