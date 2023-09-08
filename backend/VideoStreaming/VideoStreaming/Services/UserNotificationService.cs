using System.Text;

namespace VideoStreaming.Services
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UserNotificationService> _logger;

        public UserNotificationService(IEmailSender emailSender, ILogger<UserNotificationService> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task SendParticipantNotificationAsync(string email, string fullName, string ownerUserName, string roomTitle, string roomAccessCode)
        {
            var template = await File.ReadAllTextAsync("EmailTemplates/participateNotification.html");
            var builder = new StringBuilder(template);

            var html = builder
                .Replace("{title}", roomTitle)
                .Replace("{owner_username}", ownerUserName)
                .Replace("{code}", roomAccessCode)
                .ToString();

            await _emailSender.SendEmailAsync(email, "Вас добавили в комнату", html);
        }

        public async Task SendParticipantRemovingNotificationAsync(string email, string fullName, string ownerUserName, string roomTitle)
        {
            var template = await File.ReadAllTextAsync("EmailTemplates/participateRemovingNotification.html");
            var builder = new StringBuilder(template);

            var html = builder
                .Replace("{title}", roomTitle)
                .Replace("{owner_username}", ownerUserName)
                .ToString();

            await _emailSender.SendEmailAsync(email, "Вас удалили из комнаты", html);
        }
    }
}
