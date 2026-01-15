using MediatR;
using Microsoft.Extensions.Logging;
using MyApp.Application.Common.Interfaces.Triggers;
using MyApp.Application.Common.Models.Notifications;

namespace MyApp.Application.Common.Handlers.Notifications;

/// <summary>
/// Orchestrator for database entity changes
/// </summary>
public class DbEntityChangeHandler : INotificationHandler<DbEntityChangeNotification>
{
    private readonly IEnumerable<IEntityTrigger> _triggers;
    private readonly ILogger<DbEntityChangeHandler> _logger;

    public DbEntityChangeHandler(IEnumerable<IEntityTrigger> triggers, ILogger<DbEntityChangeHandler> logger)
    {
        _triggers = triggers;
        _logger = logger;
    }

    public async Task Handle(DbEntityChangeNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing DB change: {Table} {Id} {Operation}",
            notification.Table, notification.Id, notification.Operation);

        var trigger = _triggers.FirstOrDefault(t => t.EntityName.Equals(notification.Table, StringComparison.OrdinalIgnoreCase));

        if (trigger == null)
        {
            _logger.LogWarning("No application trigger found for entity: {Table}", notification.Table);
            return;
        }

        switch (notification.Operation.ToUpper())
        {
            case "INSERT":
                await trigger.OnAddAsync(notification.Id);
                break;
            case "UPDATE":
                await trigger.OnUpdateAsync(notification.Id);
                break;
            case "DELETE":
                await trigger.OnDeleteAsync(notification.Id);
                break;
            default:
                _logger.LogWarning("Unsupported operation: {Operation}", notification.Operation);
                break;
        }
    }
}
