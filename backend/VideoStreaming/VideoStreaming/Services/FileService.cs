using Microsoft.EntityFrameworkCore;
using VideoStreaming.Exceptions;
using VideoStreaming.Models;
using VideoStreaming.Persistence;

namespace VideoStreaming.Services
{
    public class FileService : IFileService
    {
        private readonly VideoStreamingDbContext _context;
        private readonly IBlobService _blobService;

        public FileService(VideoStreamingDbContext context, IBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        public async Task<FileData> UploadFileAsync(IFormFile file, string name, string description)
        {
            var blobTask = _blobService.UploadFileAsync(file);

            var result = new FileData
            {
                Description = description,
                FileName = name,
                Extension = Path.GetExtension(file.FileName).Replace(".", "")
            };

            var fileId = await blobTask;
            result.FileId = fileId;

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<FileData> GetFileDataAsync(Guid id)
        {
            var fileData = await _context.FilesData
                .FirstAsync(d => d.FileId == id);

            return fileData;
        }

        public async Task DeleteFileAsync(Guid id)
        {
            var fileData = await _context.FilesData.FirstOrDefaultAsync(d => d.FileId == id);
            if (fileData == null)
            {
                return;
            }

            var roomsCount = await _context.Rooms
                .Where(r => r.FileId == id)
                .CountAsync();
            if (roomsCount > 0)
            {
                throw new TerminateRequestException("Нельзя удалить видео, которые используются в комнатах.");
            }

            await _blobService.DeleteFileAsync(id, fileData.Extension);

            _context.Remove(fileData);
            await _context.SaveChangesAsync();
        }
    }
}
