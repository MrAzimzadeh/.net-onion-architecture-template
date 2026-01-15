using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using MyApp.Application.Common.Models.Notifications;

namespace MyApp.Persistence.Workers;

/// <summary>
/// Background worker that listens to PostgreSQL NOTIFY for generic database changes
/// AND publishes MediatR notifications for the Application layer to handle.
/// </summary>
public class PostgresNotificationWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PostgresNotificationWorker> _logger;
    private readonly string _connectionString;
    private const string ChannelName = "db_changes";

    public PostgresNotificationWorker(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<PostgresNotificationWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
        _connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("Connection string 'DefaultConnection' is missing.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Persistence Layer: Connecting to PostgreSQL on channel: {Channel}...", ChannelName);
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync(stoppingToken);

                connection.Notification += async (o, e) =>
                {
                    if (e.Channel == ChannelName)
                    {
                        try
                        {
                            using var doc = JsonDocument.Parse(e.Payload);
                            var root = doc.RootElement;

                            var tableName = root.GetProperty("table").GetString();
                            var idString = root.GetProperty("id").GetString();
                            var operation = root.GetProperty("operation").GetString();

                            if (!string.IsNullOrEmpty(tableName) &&
                                !string.IsNullOrEmpty(operation) &&
                                Guid.TryParse(idString, out var entityId))
                            {
                                // We use a scope to resolve Mediator as it might be registered as Scoped in some setups,
                                // although usually it's fine. But it's safer for future-proofing handlers.
                                using var scope = _scopeFactory.CreateScope();
                                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                                await mediator.Publish(new DbEntityChangeNotification(tableName, entityId, operation), stoppingToken);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error parsing PostgreSQL notification payload: {Payload}", e.Payload);
                        }
                    }
                };

                using (var command = new NpgsqlCommand($"LISTEN {ChannelName};", connection))
                {
                    await command.ExecuteNonQueryAsync(stoppingToken);
                }

                _logger.LogInformation("Persistence Layer: Listening for {Channel} notifications...", ChannelName);

                while (!stoppingToken.IsCancellationRequested)
                {
                    await connection.WaitAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PostgresNotificationWorker. Retrying in 5 seconds...");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
