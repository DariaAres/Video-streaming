using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoStreaming.Dtos.Out;
using VideoStreaming.Persistence;
using VideoStreaming.Services;

namespace VideoStreaming.Controllers
{
    public class FilesController : BaseAuthApiController
    {
        private readonly VideoStreamingDbContext _context;
        private readonly IMapper _mapper;

        public FilesController(VideoStreamingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<FileDataDto>> GetAllVideosAsync()
        {
            var videos = await _context.FilesData
                .ProjectTo<FileDataDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return videos;
        }

        [HttpGet("{nameTemplate}")]
        public async Task<IEnumerable<FileDataDto>> SearchByNameAsync(string nameTemplate)
        {
            var videos = await _context.FilesData
                .Where(d => d.FileName.ToLower().Contains(nameTemplate.ToLower()))
                .ProjectTo<FileDataDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return videos;
        }
    }
}
