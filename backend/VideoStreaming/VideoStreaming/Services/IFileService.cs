using VideoStreaming.Models;

namespace VideoStreaming.Services
{
    public interface IFileService
    {
        Task<FileData> UploadFileAsync(IFormFile file, string name, string description);
        Task<FileData> GetFileDataAsync(Guid id);
        Task DeleteFileAsync(Guid id);
    }
}
