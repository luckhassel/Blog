using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Blog.Domain.Messages;
using System.Text.Json;
using Blog.Domain.Interfaces.Services;
using Blog.News.Consumer.Settings;

namespace Blog.News.Consumer.Handlers
{
    public class PublishNewsMessageHandler : IConsumer<CreateNewsMessage>
    {
        private readonly IEmailService _emailService;
        private readonly ApplicationSettings _applicationSettings;
        public PublishNewsMessageHandler(IEmailService emailService, ApplicationSettings applicationSettings)
        {
            _emailService = emailService;
            _applicationSettings = applicationSettings;
        }
        public Task Consume(ConsumeContext<CreateNewsMessage> context)
        {
            _emailService.Send(
                _applicationSettings.EmailSettings.SenderName,
                _applicationSettings.EmailSettings.SenderEmail,
                _applicationSettings.EmailSettings.SenderPassword,
                context.Message.AuthorEmail, 
                context.Message.AuthorEmail, 
                $"Published news - {context.Message.Title}", 
                $"Your news '{context.Message.Title}' was published successfully",
                _applicationSettings.EmailSettings.SmtpServer,
                _applicationSettings.EmailSettings.SmtpPort
            );
            return Task.CompletedTask;
        }
    }
}