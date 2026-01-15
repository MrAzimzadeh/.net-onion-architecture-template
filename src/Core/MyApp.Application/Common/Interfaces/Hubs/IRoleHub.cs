namespace MyApp.Application.Common.Interfaces.Hubs;

/// <summary>
/// Strongly-typed SignalR client interface for Role-related notifications
/// </summary>
public interface IRoleHub
{
    Task RoleUpdated(Guid roleId, string message);
    Task RoleAdded(Guid roleId, string message);
    Task RoleDeleted(Guid roleId, string message);
}
