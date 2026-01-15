using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MyApp.Application.Common.DTOs.Mail;
using MyApp.Application.Common.Interfaces.Mail;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MyApp.Application.Common.Messaging.Constants;

namespace MyApp.Infrastructure.BackgroundServices;

public class MailConsumer : BackgroundService
{
    private readonly ILogger<MailConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    private readonly string _queueName = MessageBusConst.MailQueName;
    private readonly string _exchangeName = MessageBusConst.MailExchange;
    private readonly string _routingKey = MessageBusConst.MailRoutingKey;

    private readonly string _deadLetterExchange = MessageBusConst.DeadLetterExchange;
    private readonly string _deadLetterQueue = MessageBusConst.DeadLetterQueue;
    private readonly string _deadLetterRoutingKey = MessageBusConst.DeadLetterRoutingKey;

    private static readonly ActivitySource ActivitySource = new("RabbitMQ.Client.Consumer");

    public MailConsumer(ILogger<MailConsumer> logger, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    private async Task InitializeRabbitMQAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(exchange: _deadLetterExchange,
                                          type: ExchangeType.Direct,
                                          durable: true,
                                          autoDelete: false,
                                          cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(queue: _deadLetterQueue,
                                       durable: true,
                                       exclusive: false,
                                       autoDelete: false,
                                       arguments: null,
                                       cancellationToken: stoppingToken);

        await _channel.QueueBindAsync(queue: _deadLetterQueue,
                                     exchange: _deadLetterExchange,
                                     routingKey: _deadLetterRoutingKey,
                                     cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(exchange: _exchangeName,
                                          type: ExchangeType.Direct,
                                          durable: true,
                                          autoDelete: false,
                                          cancellationToken: stoppingToken);

        var queueArgs = new Dictionary<string, object?>
        {
            { "x-dead-letter-exchange", _deadLetterExchange },
            { "x-dead-letter-routing-key", _deadLetterRoutingKey }
        };

        await _channel.QueueDeclareAsync(queue: _queueName,
                                       durable: true,
                                       exclusive: false,
                                       autoDelete: false,
                                       arguments: queueArgs,
                                       cancellationToken: stoppingToken);

        await _channel.QueueBindAsync(queue: _queueName,
                                     exchange: _exchangeName,
                                     routingKey: _routingKey,
                                     cancellationToken: stoppingToken);

        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await InitializeRabbitMQAsync(stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel!);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                // Extract trace context from headers
                string? parentId = null;
                if (ea.BasicProperties.Headers?.TryGetValue("traceparent", out object? traceHeaderValue) == true)
                {
                    if (traceHeaderValue is byte[] bytes)
                    {
                        parentId = Encoding.UTF8.GetString(bytes);
                    }
                    else if (traceHeaderValue is string traceString)
                    {
                        parentId = traceString;
                    }
                }

                using var activity = ActivitySource.StartActivity($"Process {_queueName}", ActivityKind.Consumer, parentId);
                activity?.SetTag("messaging.system", "rabbitmq");
                activity?.SetTag("messaging.destination", _queueName);
                activity?.SetTag("messaging.operation", "receive");

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Received message from queue: {QueueName}", _queueName);

                try
                {
                    var mailRequest = JsonSerializer.Deserialize<MailRequest>(message);

                    if (mailRequest != null)
                    {
                        var success = await SendMailAsync(mailRequest);
                        if (success)
                        {
                            await _channel!.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                            _logger.LogInformation("Mail message processed and acknowledged.");
                        }
                        else
                        {
                            _logger.LogWarning("Failed to send mail. Moving message to Dead Letter Queue.");
                            await _channel!.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: stoppingToken);
                        }
                    }
                    else
                    {
                        _logger.LogError("Message deserialization returned null. Moving to DLQ.");
                        await _channel!.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while processing mail message. Moving to DLQ.");
                    activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                    await _channel!.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: stoppingToken);
                }
            };

            await _channel!.BasicConsumeAsync(queue: _queueName,
                                 autoAck: false,
                                 consumer: consumer,
                                 cancellationToken: stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Fatal error in MailConsumer background service.");
        }
    }

    private async Task<bool> SendMailAsync(MailRequest request)
    {
        using var scope = _serviceProvider.CreateScope();
        var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

        try
        {
            await mailService.SendEmailAsync(request);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending email via MailConsumer.");
            return false;
        }
    }

    public override async void Dispose()
    {
        if (_channel != null && _channel.IsOpen) await _channel.CloseAsync();
        if (_connection != null && _connection.IsOpen) await _connection.CloseAsync();
        base.Dispose();
    }
}
