namespace VideoStreaming.Dtos.Out
{
    /// <summary>
    /// DTO to show what rooms are available for user
    /// </summary>
    public class AvailableRoomDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Owner { get; set; } = default!;
        public string MovieTitle { get; set; } = default!;
        public int ParticipantsCount { get; set; }
    }
}
