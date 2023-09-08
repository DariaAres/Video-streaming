namespace VideoStreaming.Dtos.In
{
    public class AddParticipantDto
    {
        public int RoomId { get; set; }
        public string ParticipantUserName { get; set; } = default!;
    }
}
