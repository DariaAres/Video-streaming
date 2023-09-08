namespace VideoStreaming.Dtos.Out
{
    public class ParticipantDto
    {
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string UserName { get; set; } = default!;

        public string FullName { get => $"{UserName} ({Name} {Surname})"; }
    }
}
