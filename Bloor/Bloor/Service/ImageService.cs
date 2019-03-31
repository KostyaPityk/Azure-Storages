using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Bloor.Service
{
    public class ImageService
    {
        private readonly string _connectionString;

        private readonly string containerName = "images";

        public ImageService(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Public methods
        public async Task DowmloadImageAsync(string id, string directorySave)
        {
            CloudBlobContainer blobContainer = await GetCludBlobContainerAsync();
            CloudBlob cloud = blobContainer.GetBlobReference(id);

            var path = Path.Combine(directorySave, cloud.Name);

            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                await cloud.DownloadToStreamAsync(file);
            }
        }

        public async Task UploadImageAsync(string fileId, string filePath)
        {
            CloudBlockBlob blob = await GetCloudBlockBlobAsync(fileId);
            QueueService queueService = new QueueService(_connectionString);

            await blob.UploadFromStreamAsync(new FileStream(filePath, FileMode.Open));

            await queueService.Push(fileId);

        }

        public async Task UploadBlobStreamAsync(string blobName, Stream stream)
        {
            CloudBlockBlob blob = await GetCloudBlockBlobAsync(blobName);
            await blob.UploadFromStreamAsync(stream);
        }
        #endregion

        #region Private methods
        private async Task<CloudBlobContainer> GetCludBlobContainerAsync()
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);

            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

            await cloudBlobContainer.CreateIfNotExistsAsync();

            return cloudBlobContainer;
        }

        private async Task<CloudBlockBlob> GetCloudBlockBlobAsync(string blobName)
        {
            CloudBlobContainer blobContainer = await GetCludBlobContainerAsync();

            CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference(blobName);

            return cloudBlockBlob;
        }
        #endregion
    }
}
