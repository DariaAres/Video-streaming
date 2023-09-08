namespace VideoStreaming.Models
{
    public class Room : BaseEntity
    {
        public string Title { get; set; } = default!;

        public string OwnerId { get; set; } = default!;
        public User? Owner { get; set; }

        public bool VideoStarted { get; set; }

        public Guid FileId { get; set; }
        public FileData? File { get; set; }

        public ICollection<Participant>? Participants { get; set; }
    }
}
