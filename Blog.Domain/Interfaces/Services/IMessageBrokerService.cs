using Blog.Domain.Messages;
using Blog.Domain.Models.Shared;

namespace Blog.Domain.Interfaces.Services
{
    public interface IMessageBrokerService
    {
        Task<Result> Send<TMessage>(string brokerType, string name, TMessage message)
            where TMessage : Message;
    }
}