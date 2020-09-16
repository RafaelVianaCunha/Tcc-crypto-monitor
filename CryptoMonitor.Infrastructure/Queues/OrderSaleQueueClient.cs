using System.Text;
using System.Threading.Tasks;
using CryptoMonitor.Domain.Clients;
using CryptoMonitor.Domain.ValueObjects;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace CryptoMonitor.Infraestructure.Queues
{
    public class OrderSaleQueueClient : IOrderSaleQueueClient
    {
        public IQueueClient QueueClient { get; }

        public OrderSaleQueueClient(IQueueClient queueClient)
        {
            QueueClient = queueClient;
        }

        public async Task Queue(OrderSale orderSale)
        {
            var newSalesOrderMessageBody = JsonConvert.SerializeObject(orderSale);
            
            var newSalesOrderMessage = new Message(Encoding.UTF8.GetBytes(newSalesOrderMessageBody));
            
            await QueueClient.SendAsync(newSalesOrderMessage);
        }
    }
}