using MyApp.Domain.Entities;

namespace MyApp.Application.Common.Interfaces.Repositories.Write;

/// <summary>
/// User write repository interface
/// </summary>
public interface IUserWriteRepository : IWriteRepository<User>
{
    // Device session management
    Task AddDeviceSessionAsync(UserDeviceSession session, CancellationToken cancellationToken = default);
    Task UpdateDeviceSessionAsync(UserDeviceSession session, CancellationToken cancellationToken = default);
    Task DeactivateDeviceSessionAsync(Guid userId, string deviceId, CancellationToken cancellationToken = default);
    Task DeactivateAllDeviceSessionsAsync(Guid userId, CancellationToken cancellationToken = default);
    
    // External login management
    Task AddExternalLoginAsync(UserExternalLogin externalLogin, CancellationToken cancellationToken = default);
}
