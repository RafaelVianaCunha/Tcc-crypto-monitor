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
using Bitfinex.Net;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Azure.ServiceBus;

namespace CryptoMonitor
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static async Task Main(string[] args)
        {
            GetAppSettingsFile(); 

            var serviceBusConnectionString = Configuration.GetSection("ServiceBusConnectionString").Value;
            var queueName = Configuration.GetSection("NewSalesOrderQueue").Value;

            var queueClient = new QueueClient(serviceBusConnectionString, queueName);

            var stopLimitProcessor = new StopLimitProcessor(new List<IExchangeClient>
            {
                new BinanceExchangeClient(new BinanceSocketClient()),
                new BittrexExchangeClient(new BittrexSocketClient()),
                new BitfinexExchangeClient(new BitfinexSocketClient())
            },
            new PriceCalculator(),
            queueClient);

            await stopLimitProcessor.Process();

            Console.ReadLine();

            Console.ReadKey();
            Console.WriteLine("Hello World!");
        }

        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }
    }
}
