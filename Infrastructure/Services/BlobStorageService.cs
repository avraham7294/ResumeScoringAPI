using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AIResumeScoringAPI.Infrastructure.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = configuration["AzureStorage:ContainerName"];
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainer.GetBlobClient(fileName);

            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = "application/pdf" });

            return blobClient.Uri.ToString(); // Returns the file URL
        }

        public async Task<Stream?> DownloadFileAsync(string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainer.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                var downloadInfo = await blobClient.DownloadAsync();
                return downloadInfo.Value.Content;
            }
            return null;
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainer.GetBlobClient(fileName);
            return await blobClient.DeleteIfExistsAsync();
        }
    }
}