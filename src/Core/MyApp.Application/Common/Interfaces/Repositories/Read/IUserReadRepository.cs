using MyApp.Application.Common.Interfaces.Repositories.Read;
using MyApp.Domain.Entities;

namespace MyApp.Application.Common.Interfaces;

/// <summary>
/// User read repository interface
/// </summary>
public interface IUserReadRepository : IReadRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailWithRolesAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByExternalLoginAsync(string provider, string providerKey, CancellationToken cancellationToken = default);
    Task<List<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    // Device session methods
    Task<UserDeviceSession?> GetDeviceSessionAsync(Guid userId, string deviceId, CancellationToken cancellationToken = default);
    Task<UserDeviceSession?> GetDeviceSessionByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<List<UserDeviceSession>> GetUserDeviceSessionsAsync(Guid userId, CancellationToken cancellationToken = default);
}
