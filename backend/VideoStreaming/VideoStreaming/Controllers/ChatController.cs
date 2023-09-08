using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using VideoStreaming.Dtos.In;
using VideoStreaming.Dtos.Out;
using VideoStreaming.Exceptions;
using VideoStreaming.Models;
using VideoStreaming.Persistence;

namespace VideoStreaming.Controllers
{
    public class ChatController : BaseAuthApiController
    {
        private readonly VideoStreamingDbContext _context;
        private readonly IMapper _mapper;

        public ChatController(VideoStreamingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{roomId:int}")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetRoomMessagesAsync(int roomId)
        {
            var room = await _context.Rooms
                .Include(r => r.Participants)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null || room.OwnerId != CurrentUser.Id && room.Participants!.Count(p => p.UserId == CurrentUser.Id) == 0)
            {
                throw new TerminateRequestException("Невозможно найти данную комнату.", HttpStatusCode.NotFound);
            }

            var messages = await _context.ChatMessages
                .Where(m => m.RoomId == roomId)
                .OrderBy(m => m.Date)
                .ProjectTo<ChatMessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            messages.ForEach(m => m.Date = m.Date.AddHours(3));

            return messages;
        }

        [HttpPost]
        public async Task<ActionResult> SendMessageAsync(SendMessageDto dto)
        {
            var room = await _context.Rooms
                .Include(r => r.Participants)
                .FirstOrDefaultAsync(r => r.Id == dto.RoomId);
            if (room == null || room.OwnerId != CurrentUser.Id && room.Participants!.Count(p => p.UserId == CurrentUser.Id) == 0)
            {
                throw new TerminateRequestException("Невозможно найти данную комнату.", HttpStatusCode.NotFound);
            }

            var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            var message = new ChatMessage
            {
                Date = DateTime.UtcNow,
                RoomId = dto.RoomId,
                Text = dto.Text,
                UserId = CurrentUser.Id
            };

            await _context.AddAsync(message);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok();
        }
    }
}
