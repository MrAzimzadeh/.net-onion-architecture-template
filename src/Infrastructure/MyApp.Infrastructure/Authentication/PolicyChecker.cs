using System.Collections.Generic;
using System.Threading.Tasks;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;
using MyApp.Application.Common.Interfaces.Repositories.Read;
using MyApp.Domain.Entities;

namespace MyApp.Infrastructure.Authentication;

public class PolicyChecker(IRoleReadRepository readRepository) : IPolicyChecker
{
    public async Task<bool> HasPermissionAsync(List<string> roles, string permission)
    {
        var hasPermission = await readRepository.HasPermissionAsync(roles, permission);

        return hasPermission;
    }
}
