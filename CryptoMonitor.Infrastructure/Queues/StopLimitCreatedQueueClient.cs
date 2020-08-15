using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoMonitor.Domain.Clients;
using CryptoMonitor.Domain.Entities;
using CryptoMonitor.Domain.Services.Interfaces;
using CryptoMonitor.Domain.ValueObjects;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace CryptoMonitor.Infraestructure.Queues
{
    public class StopLimitCreatedQueueClient : IStopLimitCreatedQueueClient
    {
        public IQueueClient QueueClient { get; }

        public IStopLimitOrderService StopLimitOrderService { get; }

        public StopLimitCreatedQueueClient(IQueueClient queueClient, IStopLimitOrderService stopLimitOrderService)
        {
            QueueClient = queueClient;
            StopLimitOrderService = stopLimitOrderService;
        }

        public void Consume()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            QueueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            var stopLimit = JsonConvert.DeserializeObject<StopLimit>(Encoding.UTF8.GetString(message.Body));

            await Task.Run(() => StopLimitOrderService.Monitor(stopLimit));

            await QueueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}