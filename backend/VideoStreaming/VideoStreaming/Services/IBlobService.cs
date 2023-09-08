using Azure.Storage.Blobs.Models;

namespace VideoStreaming.Services
{
    public interface IBlobService
    {
        Task<Guid> UploadFileAsync(IFormFile file);
        Task<BlobItem?> GetFileAsync(Guid fileId);
        Task DeleteFileAsync(Guid fileId, string extension);
    }
}
