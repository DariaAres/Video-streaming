using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoStreaming.Constants;
using VideoStreaming.Dtos.In;
using VideoStreaming.Dtos.Out;
using VideoStreaming.Exceptions;
using VideoStreaming.Persistence;
using VideoStreaming.Services;

namespace VideoStreaming.Controllers
{
    [Authorize(Roles = AuthenticationConstants.Roles.Admin)]
    public class AdminFilesController : BaseApiController
    {
        private readonly VideoStreamingDbContext _context;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public AdminFilesController(VideoStreamingDbContext context, IFileService fileService, IMapper mapper)
        {
            _context = context;
            _fileService = fileService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<FileDataDto>> GetAllAsync()
        {
            var videos = await _context.FilesData
                .ProjectTo<FileDataDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return videos;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> UploadAsync([FromForm] UploadVideoDto dto)
        {
            var videoExtensions = new List<string>()
            {
                "mkv", "mp4", "avi", "mov"
            };

            var extension = Path.GetExtension(dto.File.FileName).Replace(".", "").ToLower();

            if (!videoExtensions.Contains(extension))
            {
                throw new TerminateRequestException("Некорректный формат файла.");
            }

            await _fileService.UploadFileAsync(dto.File, dto.Name, dto.Description);

            return Ok();
        }

        [HttpDelete("{fileId}")]
        public async Task<ActionResult> DeleteAsync(Guid fileId)
        {
            await _fileService.DeleteFileAsync(fileId);
            return Ok();
        }
    }
}
