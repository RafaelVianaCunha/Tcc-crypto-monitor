using System;
using System.Threading;
using System.Threading.Tasks;
using Bitfinex.Client.Websocket;
using Bitfinex.Client.Websocket.Client;
using Bitfinex.Client.Websocket.Requests.Subscriptions;
using Bitfinex.Client.Websocket.Websockets;
using Websocket.Client;
using Newtonsoft.Json;
using System.Collections.Generic;

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

            // Console.ReadKey();
            // Console.WriteLine("Hello World!");

            var calculator = new Calculator(97.0M);

            var task1 = Task.Run(() => {
                for (var i = 0; i < 100; i++)
                {
                    var x = new Random().Next(95, 101);
                    calculator.CalculateForNewValue(x, "Binance");
                }
            });

            var task2 = Task.Run(() => {
                for (var i = 0; i < 100; i++)
                {
                    var x = new Random().Next(95, 101);
                    calculator.CalculateForNewValue(x, "Bitfinex");
                }
            });

            var task3 = Task.Run(() => {
                for (var i = 0; i < 100; i++)
                {
                    var x = new Random().Next(95, 101);
                    calculator.CalculateForNewValue(x, "Bybit");
                }
            });

            await Task.WhenAll(task1, task2, task3);
        }
    }
}
