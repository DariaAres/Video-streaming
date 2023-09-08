using AutoMapper;
using VideoStreaming.Dtos.In;
using VideoStreaming.Dtos.Out;
using VideoStreaming.Models;

namespace VideoStreaming.Common
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<SignUpDto, User>();

            CreateMap<Room, AvailableRoomDto>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src =>
                    src.Owner!.UserName ?? "anonimus"))
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src =>
                    src.File!.FileName))
                .ForMember(dest => dest.ParticipantsCount, opt => opt.MapFrom(src =>
                    src.Participants!.Count));
            CreateMap<Room, FullRoomDto>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src =>
                    src.FileId + "." + (src.File!.Extension ?? "mp4")))
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src =>
                    src.File!.FileName));

            CreateMap<Participant, ParticipantDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    src.User!.Name ?? "anonimus"))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src =>
                    src.User!.Surname ?? "anonimus"))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                    src.User!.UserName ?? "anonimus"));

            CreateMap<ChatMessage, ChatMessageDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                    src.User!.UserName ?? "anonimus"));

            CreateMap<FileData, FileDataDto>();

            CreateMap<User, UserDto>();
        }
    }
}
