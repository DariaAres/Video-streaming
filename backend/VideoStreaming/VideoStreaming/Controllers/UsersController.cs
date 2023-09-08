using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VideoStreaming.Constants;
using VideoStreaming.Dtos.In;
using VideoStreaming.Dtos.Out;
using VideoStreaming.Exceptions;
using VideoStreaming.Persistence;

namespace VideoStreaming.Controllers
{
    public class UsersController : BaseAuthApiController
    {
        private readonly VideoStreamingDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(VideoStreamingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("exceptParticipants/{roomId:int}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersExceptRoomParticipantsAsync(int roomId)
        {
            var count = await _context.Rooms.CountAsync(r => r.Id == roomId);
            if (count == 0)
            {
                throw new TerminateRequestException("Невозможно найти данную комнату.", HttpStatusCode.NotFound);
            }

            var normalizedUserNames = await _context.Participants
                .Include(p => p.User)
                .Select(p => p.User!.NormalizedUserName)
                .ToListAsync();

            normalizedUserNames.Add(CurrentUser.NormalizedUserName);
            normalizedUserNames.Add(AuthenticationConstants.Roles.Admin.ToUpper());

            var unique = normalizedUserNames.Distinct();

            var users = await _context.Users
                .Where(u => !normalizedUserNames.Contains(u.NormalizedUserName))
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return users;
        }

        [HttpPost("profile")]
        public async Task<ActionResult> UpdateProfileAsync(UpdateProfileDto dto)
        {
            var user = await _context.Users.FirstAsync(u => u.Id == CurrentUser.Id);

            user.Name = dto.Name;
            user.Surname = dto.Surname;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
