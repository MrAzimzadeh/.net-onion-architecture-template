namespace MyApp.Application.Common.Interfaces.Hubs;

/// <summary>
/// Strongly-typed SignalR client interface for User-related notifications
/// </summary>
public interface IUserHub
{
    Task UserUpdated(Guid userId, string message);
    Task UserAdded(Guid userId, string message);
    Task UserDeleted(Guid userId, string message);
}
