using System;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using Bittrex.Net;
using Bitfinex.Net;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Azure.ServiceBus;
using CryptoMonitor.Domain.Services;
using CryptoMonitor.Domain.Interfaces;
using CryptoMonitor.Infraestructure.Queues;
using CryptoMonitor.Infraestructure.Clients;
using CryptoMonitor.Infraestructure.Repositories;

namespace CryptoMonitor
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static async Task Main(string[] args)
        {
            GetAppSettingsFile(); 

            var serviceBusConnectionString = Configuration.GetSection("ServiceBusConnectionString").Value;
            var queueName = Configuration.GetSection("NewStopLimitOrderSaleQueue").Value;

            var queueClient = new QueueClient(serviceBusConnectionString, queueName);

            var orderSaleQueueClient = new OrderSaleQueueClient(queueClient);

            var stopLimitOrderService = new StopLimitOrderService(new List<IExchangeClient>
            {
                new BinanceExchangeClient(new BinanceSocketClient()),
                new BittrexExchangeClient(new BittrexSocketClient()),
                new BitfinexExchangeClient(new BitfinexSocketClient())
            },
            new PriceCalculator(),
            new StopLimitRepository(),
            orderSaleQueueClient);

            var stopLimitCreatedQueueName = Configuration.GetSection("StopLimitCreatedQueue").Value;
            var stopLimitCreatedQueueClient = new QueueClient(serviceBusConnectionString, stopLimitCreatedQueueName);
            var stopLimitCreatedQueue = new StopLimitCreatedQueueClient(
                stopLimitCreatedQueueClient,
                stopLimitOrderService);

            stopLimitCreatedQueue.Consume();

            stopLimitOrderService.Monitor(new Domain.Entities.StopLimit
            {Stop = 19000});

            Console.ReadLine();

            Console.ReadKey();
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
