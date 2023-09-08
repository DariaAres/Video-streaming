namespace VideoStreaming.Options
{
    public class GmailSettings
    {
        public const string configurationSection = "gmail";

        public string Email { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
