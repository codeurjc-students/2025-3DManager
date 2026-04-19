using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.File;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;

namespace _3DMANAGER_APP.BLL.Services
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly BlobContainerClient _container;
        private readonly ILogger<AzureBlobStorageService> _logger;

        public AzureBlobStorageService(BlobServiceClient blobServiceClient, string containerName, ILogger<AzureBlobStorageService> logger)
        {
            _container = blobServiceClient.GetBlobContainerClient(containerName);
            _container.CreateIfNotExists(PublicAccessType.Blob);
            _logger = logger;
        }

        public async Task<FileResponse?> UploadImageAsync(Stream fileStream, string fileName, string contentType, string folder, int? groupId)
        {
            try
            {
                string key = groupId == null
                    ? $"{folder}/{Guid.NewGuid()}_{fileName}"
                    : $"group_{groupId}/{folder}/{Guid.NewGuid()}_{fileName}";

                var blobClient = _container.GetBlobClient(key);

                var headers = new BlobHttpHeaders { ContentType = contentType };

                await blobClient.UploadAsync(fileStream, headers);

                return new FileResponse
                {
                    FileKey = key,
                    FileUrl = blobClient.Uri.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AZURE BLOB ERROR");
                return null;
            }
        }

        public async Task DeleteImageAsync(string key)
        {
            await _container.DeleteBlobIfExistsAsync(key);
        }

        public async Task DeleteGroupAsync(int groupId)
        {
            string prefix = $"group_{groupId}/";

            await foreach (var blob in _container.GetBlobsAsync(
                traits: BlobTraits.None,
                states: BlobStates.None,
                prefix: prefix,
                cancellationToken: CancellationToken.None))
            {
                await _container.DeleteBlobIfExistsAsync(blob.Name);
            }
        }



        public string GetPresignedUrl(string key, int hours = 1)
        {
            var blobClient = _container.GetBlobClient(key);

            if (!blobClient.CanGenerateSasUri)
                throw new InvalidOperationException("No se puede generar SAS. Usa una connection string con permisos.");

            var sasBuilder = new BlobSasBuilder
            {
                BlobName = key,
                BlobContainerName = _container.Name,
                Resource = "b",
                ExpiresOn = DateTime.UtcNow.AddHours(hours)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

            return sasUri.ToString();
        }
    }
}
