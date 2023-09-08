namespace VideoStreaming.Dtos.In
{
    public class CreateRoomDto
    {
        public string Title { get; set; } = default!;
        public Guid FileId { get; set; }

        public IEnumerable<string> ParticipantEmails { get; set; } = new string[0];
    }
}
