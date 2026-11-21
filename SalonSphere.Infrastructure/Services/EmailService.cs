using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;

namespace SalonSphere.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string toEmail,string subject,string htmlMessage)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_settings.DisplayName,_settings.Email));

            email.To.Add(MailboxAddress.Parse(toEmail));

            email.Subject = subject;

            email.Body = new BodyBuilder
            {
                HtmlBody = htmlMessage
            }.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Timeout = 30000;

            try
            {
                await smtp.ConnectAsync(_settings.Host,_settings.Port,SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(_settings.Email,_settings.Password.Trim().Replace(" ", ""));

                await smtp.SendAsync(email);
            }
            finally
            {
                if (smtp.IsConnected)
                {
                    await smtp.DisconnectAsync(true);
                }
            }
        }
    }
}