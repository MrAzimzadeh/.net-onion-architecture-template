using Dapper;
using MyApp.Application.Common.Interfaces.Repositories.Write;
using MyApp.Domain.Entities;
using MyApp.Persistence.Database;
using MyApp.Persistence.Repositories.Base;

namespace MyApp.Persistence.Repositories;

/// <summary>
/// Role write repository implementation
/// </summary>
public class RoleWriteRepository(IDbConnectionFactory connectionFactory) : WriteRepository<Role>(connectionFactory, "\"Roles\""), IRoleWriteRepository
{
    public override async Task<bool> AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO ""Roles"" (""Id"", ""Name"", ""Description"", ""CreatedBy"", ""CreationAt"")
            VALUES (@Id, @Name, @Description, @CreatedBy, @CreationAt);";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var rowsAffected = await connection.ExecuteAsync(sql, role);
        return rowsAffected > 0;
    }

    public override async Task<bool> UpdateAsync(Role role, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE ""Roles"" 
            SET Name = @Name,
                Description = @Description,
                LastModifiedBy = @LastModifiedBy,
                LastModifiedAt = @LastModifiedAt
            WHERE Id = @Id;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var rowsAffected = await connection.ExecuteAsync(sql, role);
        return rowsAffected > 0;
    }

    public override async Task<bool> RemoveAsync(Role role, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            DELETE FROM ""Roles"" WHERE ""Id"" = @Id;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var rowsAffected = await connection.ExecuteAsync(sql, new { role.Id });
        return rowsAffected > 0;
    }

    public async Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO ""RolePolicies"" (""Id"", ""RoleId"", ""PolicyId"", ""CreatedBy"", ""CreationAt"", ""IsActive"")
            VALUES (@Id, @RoleId, @PolicyId, @CreatedBy, @CreatedAt, true);";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(sql, new
        {
            Id = Guid.NewGuid(),
            RoleId = roleId,
            PermissionId = permissionId,
            CreatedBy = Guid.Empty,
            CreatedAt = DateTime.UtcNow
        });
    }
}
