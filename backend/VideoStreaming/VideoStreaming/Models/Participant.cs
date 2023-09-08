namespace VideoStreaming.Models
{
    public class Participant
    {
        public string UserId { get; set; } = default!;
        public User? User { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public string ConfirmationCode { get; set; } = default!;
    }
}
