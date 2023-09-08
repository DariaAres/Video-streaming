using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VideoStreaming.Dtos.In;
using VideoStreaming.Dtos.Out;
using VideoStreaming.Exceptions;
using VideoStreaming.Models;
using VideoStreaming.Persistence;
using VideoStreaming.Services;

namespace VideoStreaming.Controllers
{
    public class RoomsController : BaseAuthApiController
    {
        private readonly VideoStreamingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomsController> _logger;
        private readonly IUserNotificationService _notificationService;
        private readonly IConfirmationCodeService _confirmationCodeService;
        private readonly IBlobService _blobService;
        private readonly IFileService _fileService;

        public RoomsController(
            VideoStreamingDbContext context,
            IMapper mapper,
            ILogger<RoomsController> logger,
            IUserNotificationService notificationService,
            IConfirmationCodeService confirmationCodeService,
            IBlobService blobService,
            IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _notificationService = notificationService;
            _confirmationCodeService = confirmationCodeService;
            _blobService = blobService;
            _fileService = fileService;
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<AvailableRoomDto>>> GetAvailableAsync()
        {
            var rooms = await _context.Participants
                .Include(p => p.Room)
                .ThenInclude(r => r!.File)
                .Include(p => p.Room)
                .ThenInclude(r => r!.Participants)
                .Where(p => p.UserId == CurrentUser.Id)
                .Select(p => p.Room)
                .ProjectTo<AvailableRoomDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return rooms;
        }

        [HttpGet("available/{id:int}")]
        public async Task<ActionResult<AvailableRoomDto>> GetAvailableByIdAsync(int id)
        {
            var room = await _context.Participants
                .Include(p => p.Room)
                .ThenInclude(r => r!.File)
                .Include(p => p.Room)
                .ThenInclude(r => r!.Participants)
                .Where(p => p.UserId == CurrentUser.Id)
                .Select(p => p.Room)
                .ProjectTo<AvailableRoomDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                throw new TerminateRequestException("Невозможно найти данную комнату", HttpStatusCode.NotFound);
            }

            return room;
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<FullRoomDto>>> GetMyRoomsAsync()
        {
            var rooms = await _context.Rooms
                .Where(p => p.OwnerId == CurrentUser.Id)
                .ProjectTo<FullRoomDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return rooms;
        }

        [HttpGet("full/{roomId:int}")]
        public async Task<ActionResult<FullRoomDto>> GetRoomInfoAsync(int roomId)
        {
            var participant = await _context.Participants
                .FirstOrDefaultAsync(p => p.RoomId == roomId);
            
            var room = await _context.Rooms
                .Include(r => r.Participants)
                .Include(r => r.File)
                .FirstAsync(r => r.Id == roomId);

            if (participant == null && room.OwnerId != CurrentUser.Id)
            {
                throw new TerminateRequestException("Некорректный идентификатор комнаты или код подтверждения.");
            }

            var dto = _mapper.Map<FullRoomDto>(room);

            dto.CanPlay = room.OwnerId == CurrentUser.Id;

            return dto;
        }

        [HttpGet("checkCode/{roomId:int}/{code}")]
        public async Task<ActionResult> CheckRoomCodeAsync(int roomId, string code)
        {
            var participant = await _context.Participants
                .FirstOrDefaultAsync(p => p.RoomId == roomId && p.ConfirmationCode == code);
            if (participant == null)
            {
                throw new TerminateRequestException("Некорректный идентификатор комнаты или код подтверждения.");
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CreateRoomDto dto)
        {
            var normalizedEmails = dto.ParticipantEmails
                .Select(e => e.ToUpper())
                .Where(e => e != CurrentUser.NormalizedEmail);

            // TODO:
            // add check users existent (50/50)
            var userParticipants = await _context.Users
                .Where(u => normalizedEmails.Contains(u.NormalizedEmail))
                .ToListAsync();

            var transaction = await _context.Database.BeginTransactionAsync();

            var room = new Room
            {
                OwnerId = CurrentUser.Id,
                Title = dto.Title,
                FileId = dto.FileId,
            };

            await _context.AddAsync(room);
            await _context.SaveChangesAsync();

            var participants = userParticipants.Select(perticipant => new Participant
            {
                ConfirmationCode = _confirmationCodeService.GenerateCode(),
                RoomId = room.Id,
                User = perticipant
            });

            await _context.AddRangeAsync(participants);
            await _context.SaveChangesAsync();

            foreach (var p in participants)
            {
                await _notificationService.SendParticipantNotificationAsync(
                    p.User!.Email,
                    $"{p.User.Name} {p.User.Surname}",
                    CurrentUser.UserName,
                    room.Title,
                    p.ConfirmationCode);
            }

            await transaction.CommitAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(room => room.Id == id);
            if (room == null || room.OwnerId != CurrentUser.Id)
            {
                throw new TerminateRequestException("Данной комнаты не существует.", HttpStatusCode.NotFound);
            }

            _context.Remove(room);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("participant")]
        public async Task<ActionResult> AddParticipantAsync(AddParticipantDto dto)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == dto.RoomId && r.OwnerId == CurrentUser.Id);
            if (room == null)
            {
                throw new TerminateRequestException("Невозможно найти комнату.", HttpStatusCode.NotFound);
            }

            var userParticipant = await _context.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == dto.ParticipantUserName.ToUpper());
            if (userParticipant == null)
            {
                throw new TerminateRequestException("Невозможно найти пользователя.", HttpStatusCode.NotFound);
            }

            var participant = new Participant
            {
                ConfirmationCode = _confirmationCodeService.GenerateCode(),
                RoomId = room.Id,
                UserId = userParticipant.Id
            };

            var transaction = await _context.Database.BeginTransactionAsync();

            await _context.AddAsync(participant);
            await _context.SaveChangesAsync();

            await _notificationService.SendParticipantNotificationAsync(
                userParticipant.Email,
                userParticipant.FullName,
                CurrentUser.UserName,
                room.Title,
                participant.ConfirmationCode);

            await transaction.CommitAsync();

            return Ok();
        }

        [HttpDelete("patricipant/{roomId:int}/{participantUserName}")]
        public async Task<ActionResult> RemoveParticipantAsync(int roomId, string participantUserName)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == roomId && r.OwnerId == CurrentUser.Id);
            if (room == null)
            {
                throw new TerminateRequestException("Невозможно найти комнату.", HttpStatusCode.NotFound);
            }

            var participant = await _context.Participants
                .Include(p => p.User)
                .FirstOrDefaultAsync(u => u.User!.NormalizedUserName == participantUserName.ToUpper());
            if (participant == null || participant.User == null)
            {
                throw new TerminateRequestException("Невозможно найти пользователя.", HttpStatusCode.NotFound);
            }

            var transaction = await _context.Database.BeginTransactionAsync();

            _context.Remove(participant);
            await _context.SaveChangesAsync();

            await _notificationService.SendParticipantRemovingNotificationAsync(
                participant.User.Email,
                participant.User.FullName,
                CurrentUser.UserName,
                room.Title);

            await transaction.CommitAsync();

            return Ok();
        }

        [HttpPost("startVideo/{roomId}")]
        public async Task<ActionResult> StartVideoAsync(int roomId)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == roomId && r.OwnerId == CurrentUser.Id);
            if (room == null)
            {
                throw new TerminateRequestException("Невозможно найти комнату.", HttpStatusCode.NotFound);
            }

            room.VideoStarted = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("stopVideo/{roomId}")]
        public async Task<ActionResult> StopVideoAsync(int roomId)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == roomId && r.OwnerId == CurrentUser.Id);
            if (room == null)
            {
                throw new TerminateRequestException("Невозможно найти комнату.", HttpStatusCode.NotFound);
            }

            room.VideoStarted = false;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
