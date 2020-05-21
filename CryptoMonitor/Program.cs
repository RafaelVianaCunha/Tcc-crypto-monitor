using System;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net;
using Newtonsoft.Json;
using CryptoMonitor.Clients;
using System.Collections.Generic;
using IExchangeClient = CryptoMonitor.Clients.IExchangeClient;
using CryptoMonitor.Services;
using BinanceExchangeClient = CryptoMonitor.Clients.BinanceExchangeClient;
using Bittrex.Net;
using BittrexExchangeClient = CryptoMonitor.Clients.BittrexExchangeClient;

namespace CryptoMonitor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stopLimitProcessor = new StopLimitProcessor(new List<IExchangeClient> {
                new BinanceExchangeClient(new BinanceSocketClient()),
                new BittrexExchangeClient(new BittrexSocketClient())
            });

            await stopLimitProcessor.Process();

            Console.ReadLine();

            Console.ReadKey();
            Console.WriteLine("Hello World!");
        }
    }
}
