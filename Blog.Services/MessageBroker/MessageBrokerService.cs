using Blog.Domain.Interfaces.Services;
using Blog.Domain.Messages;
using Blog.Domain.Models.Shared;
using MassTransit;

namespace Blog.Services.MessageBroker
{
    public class MessageBrokerService : IMessageBrokerService
    {
        private readonly IBus _bus;

        public MessageBrokerService(IBus bus)
        {
            _bus = bus;
        }

        public async Task<Result> Send<TMessage>(string brokerType, string name, TMessage message)
            where TMessage : Message
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri($"{brokerType}:{name}"));

            if (endpoint is null)
                return Result.Failure(Error.Create(1, "Failed sending news to it's service"));

            await endpoint.Send(message);

            return Result.Success();
        }
    }
}