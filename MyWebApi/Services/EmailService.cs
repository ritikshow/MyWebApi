using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using MyWebApi.Configurations;

public class EmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));

        if (string.IsNullOrWhiteSpace(_emailSettings.SenderEmail))
            throw new ArgumentNullException(nameof(_emailSettings.SenderEmail), "Sender email is not configured.");
    }

    public void SendEmail(string subject, string body)
    {
        try
        {
            using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer))
            {
                smtpClient.Port = _emailSettings.Port;
                smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(_emailSettings.RecipientEmail);

                smtpClient.Send(mailMessage);
                Console.WriteLine("✅ Email sent successfully!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error sending email: {ex.Message}");
        }
    }
}
