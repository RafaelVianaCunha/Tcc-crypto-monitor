using System;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net;
using Bittrex.Net;
using CryptoExchange.Net.Sockets;
using Newtonsoft;
using Newtonsoft.Json;

namespace CryptoMonitor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // var exitEvent = new ManualResetEvent(false);
            // string coin = "BTC-USDT";

            // var task1 = Task.Run(() => {
            //     using(var client = new BinanceSocketClient()) {
            //     var successSymbols = client.SubscribeToAllSymbolTickerUpdates((data) =>
            //     {
            //         foreach (var item in data)
            //         {
            //             Console.WriteLine("Binance " + item.BestBidPrice);
            //         }
            //     });
            // }});

            // var socketClient = new BittrexSocketClient();
            // socketClient.SubscribeToOrderBookUpdates("BTC-ETH", (data) => {
            //         foreach(var item in data.Buys) {
            //             Console.WriteLine(item.Price);
            //         }
            // });

            // socketClient.SubscribeToSymbolSummariesUpdate((data) => {
            //     foreach(var item in data) {
            //         Console.WriteLine(item.High);
            //     }
            // });

            var binanceClient = new BinanceClient(new BinanceSocketClient());
            await Task.Run(() => binanceClient.ConsumeCoinValue("BTCUSDT", (coin) => 
            {
                Console.WriteLine(JsonConvert.SerializeObject(coin));
            }));

            Console.ReadLine();

            Console.ReadKey();
            Console.WriteLine("Hello World!");

            // var calculator = new Calculator(97.0M);

            // var task1 = Task.Run(() => {
            //     for (var i = 0; i < 100; i++)
            //     {
            //         var x = new Random().Next(95, 101);
            //         calculator.CalculateForNewValue(x, "Binance");
            //     }
            // });

            // var task2 = Task.Run(() => {
            //     for (var i = 0; i < 100; i++)
            //     {
            //         var x = new Random().Next(95, 101);
            //         calculator.CalculateForNewValue(x, "Bitfinex");
            //     }
            // });

            // var task3 = Task.Run(() => {
            //     for (var i = 0; i < 100; i++)
            //     {
            //         var x = new Random().Next(95, 101);
            //         calculator.CalculateForNewValue(x, "Bybit");
            //     }
            // });

            // await Task.WhenAll(task1, task2, task3);
        }
    }
}
