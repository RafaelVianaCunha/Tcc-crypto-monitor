using System;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net;
using Bittrex.Net;
using CryptoExchange.Net.Sockets;


namespace CryptoMonitor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var exitEvent = new ManualResetEvent(false);
            // var url = BitfinexValues.ApiWebsocketUrl;

            // using (var communicator = new BitfinexWebsocketCommunicator(url))
            // {
            //     using (var client = new BitfinexWebsocketClient(communicator))
            //     {
            //         client.Streams.InfoStream.Subscribe(info =>
            //         {
            //             Console.WriteLine($"Info received, reconnection happened, resubscribing to streams");
                        
            //             client.Send(new TickerSubscribeRequest("BTC/USD"));
            //         });

            //         client.Streams.TickerStream.Subscribe(ticker => 
            //         {
            //             Console.WriteLine($"{DateTime.Now} Compra: {ticker.Bid} Venda {ticker.Ask}");
            //         });

            //         await communicator.Start();

            //         exitEvent.WaitOne();
            //     }
            // }

            // var binanceUrl = new Uri("wss://dex.binance.org/api/ws");
            // using (var ws = new WebsocketClient(binanceUrl)) {
            //     await ws.Start();
                
            //     var message = JsonConvert.SerializeObject(new {
            //         method = "subscribe",
            //         topic = "allTickers",
            //         symbols = new string[] { "$all" }
            //     });

            //     Console.WriteLine(message);

            //     ws.Send(message);

            //     ws.MessageReceived.Subscribe(msg => Console.WriteLine(msg));

            //     exitEvent.WaitOne();
            // };

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

            var socketClient = new BittrexSocketClient();
            socketClient.SubscribeToOrderBookUpdates("BTC-ETH", (data) => {
                    foreach(var item in data.Buys) {
                        Console.WriteLine(item.Price);
                    }
            });

            // socketClient.SubscribeToSymbolSummariesUpdate((data) => {
            //     foreach(var item in data) {
            //         Console.WriteLine(item.High);
            //     }
            // });

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
