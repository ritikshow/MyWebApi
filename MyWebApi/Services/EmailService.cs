using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using MyWebApi.Configurations;

namespace MyWebApi.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("My API", _emailSettings.SenderEmail));
            email.To.Add(new MailboxAddress("Recipient", _emailSettings.RecipientEmail));
            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
