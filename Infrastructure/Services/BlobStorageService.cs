using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace AIResumeScoringAPI.Infrastructure.Services
{
    /// <summary>
    /// Service for handling file operations in Azure Blob Storage.
    /// </summary>
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        /// <summary>
        /// Initializes the BlobStorageService with configuration.
        /// </summary>
        /// <param name="configuration">Application configuration containing Azure Storage settings.</param>
        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = configuration["AzureStorage:ContainerName"];
        }

        /// <summary>
        /// Uploads a PDF file to Azure Blob Storage.
        /// </summary>
        /// <param name="fileStream">Stream of the file to upload.</param>
        /// <param name="fileName">Unique file name for storage.</param>
        /// <returns>Public URL of the uploaded file.</returns>
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainer.GetBlobClient(fileName);

            // Upload file to Blob Storage
            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = "application/pdf" });

            // Return the file's public URL
            return blobClient.Uri.ToString();
        }

        /// <summary>
        /// Downloads a file from Azure Blob Storage.
        /// </summary>
        /// <param name="fileName">Name of the file to download.</param>
        /// <returns>Stream of the file if found; otherwise, null.</returns>
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

        /// <summary>
        /// Deletes a file from Azure Blob Storage.
        /// </summary>
        /// <param name="fileName">Name of the file to delete.</param>
        /// <returns>True if deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainer.GetBlobClient(fileName);

            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
