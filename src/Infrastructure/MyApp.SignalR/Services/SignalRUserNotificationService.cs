using Microsoft.AspNetCore.SignalR;
using MyApp.Application.Common.Interfaces.Hubs;
using MyApp.Application.Common.Interfaces.Notifications;
using MyApp.SignalR.Hubs;

namespace MyApp.SignalR.Services;

/// <summary>
/// SignalR implementation of User notification service
/// </summary>
public class SignalRUserNotificationService : IUserNotificationService
{
    private readonly IHubContext<UserHub, IUserHub> _hubContext;

    public SignalRUserNotificationService(IHubContext<UserHub, IUserHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task NotifyUserAddedAsync(Guid userId, string message)
        => _hubContext.Clients.All.UserAdded(userId, message);

    public Task NotifyUserUpdatedAsync(Guid userId, string message)
        => _hubContext.Clients.All.UserUpdated(userId, message);

    public Task NotifyUserDeletedAsync(Guid userId, string message)
        => _hubContext.Clients.All.UserDeleted(userId, message);
}
