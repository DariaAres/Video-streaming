namespace VideoStreaming.Dtos.In
{
    public class EmailConfirmationDto
    {
        public string Email { get; set; } = default!;
        public int Code { get; set; }
    }
}
