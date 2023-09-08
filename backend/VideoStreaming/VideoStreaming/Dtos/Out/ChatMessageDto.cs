namespace VideoStreaming.Dtos.Out
{
    public class ChatMessageDto
    {
        public string Text { get; set; } = default!;
        public DateTime Date { get; set; }
        public string UserName { get; set; } = default!;
    }
}
