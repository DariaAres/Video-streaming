namespace VideoStreaming.Dtos.In
{
    public class SendMessageDto
    {
        public int RoomId { get; set; }
        public string Text { get; set; } = default!;
    }
}
