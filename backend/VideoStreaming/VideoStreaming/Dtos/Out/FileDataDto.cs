namespace VideoStreaming.Dtos.Out
{
    public class FileDataDto
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
