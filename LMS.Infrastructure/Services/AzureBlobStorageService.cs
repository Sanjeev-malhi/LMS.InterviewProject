using Azure.Storage.Blobs;
using LMS.Application.Configuration;
using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LMS.Infrastructure.Services
{
    public class AzureBlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public AzureBlobStorageService(
            IOptions<AzureStorageSettings> options)
        {
            var settings = options.Value;

            _containerClient =
                new BlobContainerClient(
                    settings.ConnectionString,
                    settings.ContainerName);
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            BlobClient blobClient =
                _containerClient.GetBlobClient(fileName);

            using var stream = file.OpenReadStream();

            await blobClient.UploadAsync(
                stream,
                overwrite: false);

            return blobClient.Uri.ToString();
        }

        public async Task DeleteAsync(string blobName)
        {
            BlobClient blobClient =
                _containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<Stream> DownloadAsync(string blobName)
        {
            BlobClient blobClient =
                _containerClient.GetBlobClient(blobName);

            var response =
                await blobClient.DownloadStreamingAsync();

            return response.Value.Content;
        }
    }
}