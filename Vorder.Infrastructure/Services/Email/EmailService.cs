using MailKit.Net.Smtp;
using MimeKit;
using Vorder.Application.Interfaces.Services.Email;
using Vorder.Domain.Models;

namespace Vorder.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(EmailConfiguration emailConfiguration) => _emailConfiguration = emailConfiguration;

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Confirm Email", _emailConfiguration.From));
            emailMessage.To.Add(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }

        public async Task SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            await Send(emailMessage);
        }

        private async Task Send(MimeMessage mime)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfiguration.SMTP, _emailConfiguration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAuth2");
                client.Authenticate(_emailConfiguration.Username, _emailConfiguration.Password);

                await client.SendAsync(mime);
            }
            catch (Exception)
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
