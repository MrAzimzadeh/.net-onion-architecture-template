namespace MyApp.Application.Common.Interfaces.Notifications;

/// <summary>
/// Abstraction for sending role-related real-time notifications
/// </summary>
public interface IRoleNotificationService
{
    Task NotifyRoleAddedAsync(Guid roleId, string message);
    Task NotifyRoleUpdatedAsync(Guid roleId, string message);
    Task NotifyRoleDeletedAsync(Guid roleId, string message);
}
