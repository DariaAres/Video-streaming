namespace VideoStreaming.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = default!;
        public DateTime Date { get; set; }

        public string UserId { get; set; } = default!;
        public User? User { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
