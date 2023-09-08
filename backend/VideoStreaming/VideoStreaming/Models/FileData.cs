namespace VideoStreaming.Models
{
    public class FileData
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Extension { get; set; } = default!;
    }
}
