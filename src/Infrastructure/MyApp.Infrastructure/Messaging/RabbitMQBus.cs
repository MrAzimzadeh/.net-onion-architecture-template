using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MyApp.Application.Common.Interfaces.Messaging;
using RabbitMQ.Client;

namespace MyApp.Infrastructure.Messaging;

public class RabbitMQBus : IMessageBus
{
    private readonly IConfiguration _configuration;
    private static readonly ActivitySource ActivitySource = new("RabbitMQ.Client.Publisher");

    public RabbitMQBus(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task PublishAsync<T>(T message, string exchangeName, string routingKey, string? type = null) where T : class
    {
        using var activity = ActivitySource.StartActivity($"Publish {routingKey}", ActivityKind.Producer);
        activity?.SetTag("messaging.system", "rabbitmq");
        activity?.SetTag("messaging.destination", exchangeName);
        activity?.SetTag("messaging.destination_kind", "exchange");
        activity?.SetTag("messaging.rabbitmq.routing_key", routingKey);

        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: exchangeName,
                                          type: type ?? ExchangeType.Direct,
                                          durable: true,
                                          autoDelete: false);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent
        };

        // Inject trace context headers if activity is present
        if (activity != null)
        {
            properties.Headers ??= new Dictionary<string, object?>();
            properties.Headers["traceparent"] = activity.Id;
        }

        await channel.BasicPublishAsync(exchange: exchangeName,
                                       routingKey: routingKey,
                                       body: body,
                                       basicProperties: properties,
                                       mandatory: true);
    }

    public async Task PublishAsync<T>(T message, string queueName) where T : class
    {
        using var activity = ActivitySource.StartActivity($"Publish {queueName}", ActivityKind.Producer);
        activity?.SetTag("messaging.system", "rabbitmq");
        activity?.SetTag("messaging.destination", queueName);
        activity?.SetTag("messaging.destination_kind", "queue");

        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent
        };

        if (activity != null)
        {
            properties.Headers ??= new Dictionary<string, object?>();
            properties.Headers["traceparent"] = activity.Id;
        }

        await channel.BasicPublishAsync(exchange: "",
                             routingKey: queueName,
                             body: body,
                             basicProperties: properties,
                             mandatory: false);
    }
}
