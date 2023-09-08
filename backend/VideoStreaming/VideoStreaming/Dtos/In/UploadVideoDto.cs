namespace VideoStreaming.Dtos.In
{
    public class UploadVideoDto
    {
        public IFormFile File { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
