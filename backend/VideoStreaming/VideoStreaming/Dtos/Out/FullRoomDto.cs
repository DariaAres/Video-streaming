namespace VideoStreaming.Dtos.Out
{
    public class FullRoomDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public bool VideoStarted { get; set; }
        public string MovieTitle { get; set; } = default!;
        public bool CanPlay { get; set; }

        public IEnumerable<ParticipantDto> Participants { get; set; } = default!;
    }
}
