using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace VideoStreaming.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _containerClient = blobServiceClient.GetBlobContainerClient("videostreaming");
        }

        public async Task<Guid> UploadFileAsync(IFormFile file)
        {
            var fileId = Guid.NewGuid();
            var fileName = fileId.ToString() + Path.GetExtension(file.FileName).ToLower();

            using var memoryStream = new MemoryStream();

            file.CopyTo(memoryStream);
            memoryStream.Position = 0;

            var response = await _containerClient.UploadBlobAsync(fileName, memoryStream, default);

            return fileId;
        }

        public Task<BlobItem?> GetFileAsync(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteFileAsync(Guid fileId, string extension)
        {
            var blobContainerClient = _containerClient.GetBlobClient($"{fileId}.{extension}");
            var exists = blobContainerClient.Exists();
            await blobContainerClient.DeleteIfExistsAsync();
        }
    }
}
