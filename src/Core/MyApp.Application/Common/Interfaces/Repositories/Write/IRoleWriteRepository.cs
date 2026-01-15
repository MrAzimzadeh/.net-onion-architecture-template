using MyApp.Domain.Entities;

namespace MyApp.Application.Common.Interfaces.Repositories.Write;

/// <summary>
/// Role write repository interface
/// </summary>
public interface IRoleWriteRepository : IWriteRepository<Role>
{
    Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);
}
