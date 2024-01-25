using Blog.Domain.Interfaces.Services;
using Blog.Domain.Models.Shared;
using MailKit.Net.Smtp;
using MimeKit;
using Blog.Application.Settings;

namespace Blog.Services.Email
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {
        }

        public Result Send(
            string senderName,
            string senderEmail,
            string senderPassword,
            string receiverName,
            string receiverEmail,
            string subject,
            string message,
            string smtpServer,
            int smtpPort
            )
        {
            if (string.IsNullOrWhiteSpace(receiverName))
                return Result.Failure(Error.Create(1, "Receiver cannot be null"));

            if (string.IsNullOrWhiteSpace(receiverEmail))
                return Result.Failure(Error.Create(1, "Receiver email cannot be null"));

            if (string.IsNullOrWhiteSpace(message))
                return Result.Failure(Error.Create(1, "Receiver message cannot be null"));

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(senderName, senderEmail));
            email.To.Add(new MailboxAddress(receiverName, receiverEmail));

            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var smtp = new SmtpClient();
            smtp.Connect(smtpServer, smtpPort, false);

            smtp.Authenticate(senderEmail, senderPassword);

            smtp.Send(email);
            smtp.Disconnect(true);

            return Result.Success();
        }
    }
}