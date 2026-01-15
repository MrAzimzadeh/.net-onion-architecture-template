using Dapper;
using MyApp.Application.Common.Interfaces.Repositories.Read;
using MyApp.Domain.Entities;
using MyApp.Persistence.Database;
using MyApp.Persistence.Repositories.Base;

namespace MyApp.Persistence.Repositories;

/// <summary>
/// Role read repository implementation
/// </summary>
public class RoleReadRepository(IDbConnectionFactory connectionFactory) : ReadRepository<Role>(connectionFactory, "\"Roles\""), IRoleReadRepository
{
    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT * FROM ""Roles"" WHERE ""Name"" = @Name;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<Role>(sql, new { Name = name });
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT COUNT(1) FROM ""Roles"" WHERE ""Name"" = @Name;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Name = name });
        return count > 0;
    }

    public async Task<List<Policy>> GetRolePoliciesAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT p.* FROM ""Policies"" p
            INNER JOIN ""RolePolicies"" rp ON p.""Id"" = rp.""PolicyId""
            WHERE rp.""RoleId"" = @RoleId AND rp.""IsActive"" = true;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var policies = await connection.QueryAsync<Policy>(sql, new { RoleId = roleId });
        return policies.ToList();
    }

    public async Task<bool> HasPermissionAsync(List<string> roles, string permission)
    {
        const string sql = @"
            SELECT COUNT(1) FROM ""Policies"" p
            INNER JOIN ""RolePolicies"" rp ON p.""Id"" = rp.""PolicyId""
            INNER JOIN ""Roles"" r ON rp.""RoleId"" = r.""Id""
            WHERE r.""Name"" = ANY(@Roles)
            AND p.""Name"" = @Permission 
            AND rp.""IsActive"" = true;";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Roles = roles.ToArray(), Permission = permission });
        return count > 0;
    }
}
