using MyApp.Application.Common.Interfaces.Notifications;
using MyApp.Application.Common.Interfaces.Triggers;

namespace MyApp.Application.Triggers;

/// <summary>
/// Application orchestrator for Role changes
/// </summary>
public class RoleTrigger : IEntityTrigger
{
    private readonly IRoleNotificationService _notificationService;

    public RoleTrigger(IRoleNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public string EntityName => "Roles";

    public async Task OnAddAsync(Guid entityId)
    {
        await _notificationService.NotifyRoleAddedAsync(entityId, "New role created.");
    }

    public async Task OnUpdateAsync(Guid entityId)
    {
        await _notificationService.NotifyRoleUpdatedAsync(entityId, "Role updated.");
    }

    public async Task OnDeleteAsync(Guid entityId)
    {
        await _notificationService.NotifyRoleDeletedAsync(entityId, "Role deleted.");
    }
}
