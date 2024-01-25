using Blog.Domain.Models.Shared;

namespace Blog.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Result Send(string senderName,
            string senderEmail,
            string senderPassword,
            string receiverName, 
            string receiverEmail, 
            string subject, 
            string message,
            string smtpServer,
            int smtpPort);
    }
}