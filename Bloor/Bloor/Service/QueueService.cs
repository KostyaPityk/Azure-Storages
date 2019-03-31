using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Bloor.Service
{
    public class QueueService
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudQueueClient _queueClient;
        private readonly string _connectionString;
        private readonly string _queueName = "transformqueue";

        public QueueService(string connectionString)
        {
            _connectionString = connectionString;
            _storageAccount = CloudStorageAccount.Parse(_connectionString);
            _queueClient = _storageAccount.CreateCloudQueueClient();
        }

        public async Task Push(string message)
        {

            CloudQueue queue = _queueClient.GetQueueReference(_queueName);

            await queue.CreateIfNotExistsAsync();

            CloudQueueMessage queueMessage = new CloudQueueMessage(message);
            await queue.AddMessageAsync(queueMessage);
        }
    }
}
