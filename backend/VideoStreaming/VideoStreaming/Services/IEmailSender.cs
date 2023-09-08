namespace VideoStreaming.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage, params string[] attachPaths);
    }
}
