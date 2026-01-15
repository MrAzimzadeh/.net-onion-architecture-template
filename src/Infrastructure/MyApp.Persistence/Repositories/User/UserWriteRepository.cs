using Dapper;
using MyApp.Application.Common.Interfaces.Repositories.Write;
using MyApp.Domain.Entities;
using MyApp.Persistence.Database;
using MyApp.Persistence.Repositories.Base;

namespace MyApp.Persistence.Repositories;

/// <summary>
/// User write repository implementation
/// </summary>
public class UserWriteRepository(IDbConnectionFactory connectionFactory) : WriteRepository<User>(connectionFactory, "\"Users\""), IUserWriteRepository
{
    public override async Task<bool> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO ""Users"" (""Id"", ""FirstName"", ""LastName"", ""EmailAddress"", ""PhoneNumber"", 
                               ""PasswordHash"", ""PasswordSalt"", ""IsActive"", ""TenantId"", 
                               ""CreatedBy"", ""CreationAt"")
            VALUES (@Id, @FirstName, @LastName, @EmailAddress, @PhoneNumber,
                    @PasswordHash, @PasswordSalt, @IsActive, @TenantId,
                    @CreatedBy, @CreationAt);";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var rowsAffected = await connection.ExecuteAsync(sql, user);
        return rowsAffected > 0;
    }

    public override async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE ""Users"" 
            SET ""FirstName"" = @FirstName,
                ""LastName"" = @LastName,
                ""EmailAddress"" = @EmailAddress,
                ""PhoneNumber"" = @PhoneNumber,
                ""PasswordHash"" = @PasswordHash,
                ""PasswordSalt"" = @PasswordSalt,
                ""IsActive"" = @IsActive,
                ""LastModifiedBy"" = @LastModifiedBy,
                ""LastModifiedAt"" = @LastModifiedAt
            WHERE ""Id"" = @Id;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var rowsAffected = await connection.ExecuteAsync(sql, user);
        return rowsAffected > 0;
    }

    public override async Task<bool> RemoveAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE ""Users"" SET ""IsActive"" = false, ""LastModifiedAt"" = GETUTCDATE() WHERE ""Id"" = @Id;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var rowsAffected = await connection.ExecuteAsync(sql, new { user.Id });
        return rowsAffected > 0;
    }

    public async Task AddDeviceSessionAsync(UserDeviceSession session, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO ""UserDeviceSessions"" (""Id"", ""UserId"", ""DeviceId"", ""DeviceType"", ""DeviceName"", 
                                           ""RefreshToken"", ""RefreshTokenExpiresAt"", ""CreatedAt"", 
                                           ""LastUsedAt"", ""IsActive"", ""CreatedBy"", ""CreationAt"")
            VALUES (@Id, @UserId, @DeviceId, @DeviceType, @DeviceName,
                    @RefreshToken, @RefreshTokenExpiresAt, @CreatedAt,
                    @LastUsedAt, @IsActive, @CreatedBy, @CreatedAt);";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(sql, session);
    }

    public async Task UpdateDeviceSessionAsync(UserDeviceSession session, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE ""UserDeviceSessions"" 
            SET ""RefreshToken"" = @RefreshToken,
                ""RefreshTokenExpiresAt"" = @RefreshTokenExpiresAt,
                ""LastUsedAt"" = @LastUsedAt,
                ""IsActive"" = @IsActive
            WHERE ""Id"" = @Id;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(sql, session);
    }

    public async Task DeactivateDeviceSessionAsync(Guid userId, string deviceId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE ""UserDeviceSessions"" 
            SET ""IsActive"" = false, RefreshToken = NULL
            WHERE ""UserId"" = @UserId AND DeviceId = @DeviceId;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(sql, new { UserId = userId, DeviceId = deviceId });
    }

    public async Task DeactivateAllDeviceSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE ""UserDeviceSessions""  
            SET ""IsActive"" = false, RefreshToken = NULL
            WHERE ""UserId"" = @UserId;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(sql, new { UserId = userId });
    }

    public async Task AddExternalLoginAsync(UserExternalLogin externalLogin, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO ""UserExternalLogins"" (""Id"", ""UserId"", ""Provider"", ""ProviderKey"", ""CreatedAt"", ""CreatedBy"")
            VALUES (@Id, @UserId, @Provider, @ProviderKey, @CreatedAt, @CreatedBy);";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(sql, externalLogin);
    }
}
