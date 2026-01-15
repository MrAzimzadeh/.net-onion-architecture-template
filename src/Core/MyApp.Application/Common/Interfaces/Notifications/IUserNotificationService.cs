namespace MyApp.Application.Common.Interfaces.Notifications;

/// <summary>
/// Abstraction for sending user-related real-time notifications
/// </summary>
public interface IUserNotificationService
{
    Task NotifyUserAddedAsync(Guid userId, string message);
    Task NotifyUserUpdatedAsync(Guid userId, string message);
    Task NotifyUserDeletedAsync(Guid userId, string message);
}
