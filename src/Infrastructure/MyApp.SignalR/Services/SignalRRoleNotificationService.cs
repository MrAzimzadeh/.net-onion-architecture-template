using Microsoft.AspNetCore.SignalR;
using MyApp.Application.Common.Interfaces.Hubs;
using MyApp.Application.Common.Interfaces.Notifications;
using MyApp.SignalR.Hubs;

namespace MyApp.SignalR.Services;

/// <summary>
/// SignalR implementation of Role notification service
/// </summary>
public class SignalRRoleNotificationService : IRoleNotificationService
{
    private readonly IHubContext<RoleHub, IRoleHub> _hubContext;

    public SignalRRoleNotificationService(IHubContext<RoleHub, IRoleHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task NotifyRoleAddedAsync(Guid roleId, string message)
        => _hubContext.Clients.All.RoleAdded(roleId, message);

    public Task NotifyRoleUpdatedAsync(Guid roleId, string message)
        => _hubContext.Clients.All.RoleUpdated(roleId, message);

    public Task NotifyRoleDeletedAsync(Guid roleId, string message)
        => _hubContext.Clients.All.RoleDeleted(roleId, message);
}
