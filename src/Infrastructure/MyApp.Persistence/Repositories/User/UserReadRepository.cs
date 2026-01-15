using Dapper;
using MyApp.Application.Common.Interfaces;
using MyApp.Domain.Entities;
using MyApp.Persistence.Database;
using MyApp.Persistence.Repositories.Base;

namespace MyApp.Persistence.Repositories;

/// <summary>
/// User read repository implementation
/// </summary>
public class UserReadRepository(IDbConnectionFactory connectionFactory) : ReadRepository<User>(connectionFactory, "\"Users\""), IUserReadRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT * FROM ""Users"" WHERE ""EmailAddress"" = @Email;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT * FROM ""Users"" WHERE ""Id"" = @Id;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User?> GetByEmailWithRolesAsync(string email, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT * FROM ""Users"" WHERE ""EmailAddress"" = @Email;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User?> GetByExternalLoginAsync(string provider, string providerKey, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT u.* FROM ""Users"" u
            INNER JOIN ""UserExternalLogins"" uel ON u.""Id"" = uel.""UserId""
            WHERE uel.""Provider"" = @Provider AND uel.""ProviderKey"" = @ProviderKey;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Provider = provider, ProviderKey = providerKey });
    }

    public async Task<List<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT r.* FROM ""Roles"" r
            INNER JOIN ""UserRoles"" ur ON r.""Id"" = ur.""RoleId""
            WHERE ur.""UserId"" = @UserId;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var roles = await connection.QueryAsync<Role>(sql, new { UserId = userId });
        return roles.ToList();
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT COUNT(1) FROM ""Users"" WHERE ""EmailAddress"" = @Email;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });
        return count > 0;
    }

    public async Task<UserDeviceSession?> GetDeviceSessionAsync(Guid userId, string deviceId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT * FROM ""UserDeviceSessions"" 
            WHERE ""UserId"" = @UserId AND ""DeviceId"" = @DeviceId;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<UserDeviceSession>(sql, new { UserId = userId, DeviceId = deviceId });
    }

    public async Task<UserDeviceSession?> GetDeviceSessionByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT * FROM ""UserDeviceSessions"" 
            WHERE ""RefreshToken"" = @RefreshToken AND ""IsActive"" = true;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<UserDeviceSession>(sql, new { RefreshToken = refreshToken });
    }

    public async Task<List<UserDeviceSession>> GetUserDeviceSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT * FROM ""UserDeviceSessions"" 
            WHERE ""UserId"" = @UserId 
            ORDER BY ""LastUsedAt"" DESC;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var sessions = await connection.QueryAsync<UserDeviceSession>(sql, new { UserId = userId });
        return sessions.ToList();
    }
}
