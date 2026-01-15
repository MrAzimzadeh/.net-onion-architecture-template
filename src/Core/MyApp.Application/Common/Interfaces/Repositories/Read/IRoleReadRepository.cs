using MyApp.Domain.Entities;

namespace MyApp.Application.Common.Interfaces.Repositories.Read;

/// <summary>
/// Role read repository interface
/// </summary>
public interface IRoleReadRepository : IReadRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<Policy>> GetRolePoliciesAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(List<string> roles, string permission);
}
