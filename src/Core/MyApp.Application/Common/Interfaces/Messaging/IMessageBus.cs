namespace MyApp.Application.Common.Interfaces.Messaging;

public interface IMessageBus
{
    Task PublishAsync<T>(T message, string exchangeName, string routingKey, string? type = null) where T : class;
    Task PublishAsync<T>(T message, string queueName) where T : class;
}
