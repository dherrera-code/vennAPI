using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace vennAPI.Services
{
    public class BlobServices
    {
        private readonly BlobServiceClient _blobServiceClient;
    
        private readonly string _ContainerName;

        public BlobServices(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _ContainerName = configuration["AzureBlobStorage:ContainerName"];
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        // function to upload files
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_ContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            await containerClient.CreateIfNotExistsAsync();
            await blobClient.UploadAsync(fileStream, overwrite: true);

            return blobClient.Uri.ToString();
        }
    }
}