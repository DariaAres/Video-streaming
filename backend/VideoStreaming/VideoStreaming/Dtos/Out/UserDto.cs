namespace VideoStreaming.Dtos.Out
{
    public class UserDto
    {
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string UserName { get; set; } = default!;

        public string FullName { get => $"{UserName} ({Name} {Surname})"; }
    }
}
