using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using VideoStreaming.Options;

namespace VideoStreaming.Services
{
    public class GmailSender : IEmailSender
    {
        private readonly ILogger<GmailSender> _logger;

        private readonly SmtpClient _smtpClient;
        private readonly MailAddress _fromAddress;

        public GmailSender(
            ILogger<GmailSender> logger,
            IOptions<GmailSettings> gmailSettingsOptions)
        {
            _logger = logger;

            var gmailSettings = gmailSettingsOptions.Value;
            _fromAddress = new MailAddress(gmailSettings.Email, gmailSettings.Subject);
            _smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_fromAddress.Address, gmailSettings.Password),
                Timeout = 20000
            };
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, params string[] attachPaths)
        {
            var toAddress = new MailAddress(email, email);

            _logger.LogInformation("Sending email to {0}", email);

            using (var msg = new MailMessage(_fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = htmlMessage
            })
            {
                foreach (var item in attachPaths)
                {
                    msg.Attachments.Add(new Attachment(item));
                }

                await _smtpClient.SendMailAsync(msg);

                _logger.LogInformation("Email sent to {0}", email);
            }
        }
    }
}
