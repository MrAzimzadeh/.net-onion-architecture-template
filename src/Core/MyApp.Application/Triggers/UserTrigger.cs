using MyApp.Application.Common.DTOs;
using MyApp.Application.Common.Interfaces.Messaging;
using MyApp.Application.Common.Interfaces.Notifications;
using MyApp.Application.Common.Interfaces.Triggers;

namespace MyApp.Application.Triggers;

/// <summary>
/// Application orchestrator for User changes
/// </summary>
public class UserTrigger : IEntityTrigger
{
    private readonly IUserNotificationService _notificationService;
    private readonly IMessageBus _bus;

    public UserTrigger(IUserNotificationService notificationService, IMessageBus bus)
    {
        _notificationService = notificationService;
        _bus = bus;
    }

    public string EntityName => "Users";

    public async Task OnAddAsync(Guid entityId)
    {
        // Coordination logic
        await _notificationService.NotifyUserAddedAsync(entityId, "New user created.");
        // await _bus.PublishAsync(new UserCreatedEvent(entityId));
    }

    public async Task OnUpdateAsync(Guid entityId)
    {
        await _notificationService.NotifyUserUpdatedAsync(entityId, "User updated.");
        await _bus.PublishAsync(new UserDto(entityId), queueName: "user-updates");
    }

    public async Task OnDeleteAsync(Guid entityId)
    {
        await _notificationService.NotifyUserDeletedAsync(entityId, "User deleted.");
        await _bus.PublishAsync(new UserDto(entityId), queueName: "user-deletes");
    }
}
