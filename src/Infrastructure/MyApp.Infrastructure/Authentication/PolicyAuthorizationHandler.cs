using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;
using MyApp.Application.Security;


namespace MyApp.Infrastructure.Authentication;

public class PolicyAuthorizationHandler : AuthorizationHandler<PolicyRequirement>
{
    private readonly IPolicyChecker _permissionChecker;

    public PolicyAuthorizationHandler(IPolicyChecker permissionChecker)
    {
        _permissionChecker = permissionChecker;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
    {
        if (context.User == null) return;

        // If user has the permission claim directly (Implementation A support)
        if (context.User.HasClaim(c => c.Type == PolicyConstants.PolicyClaimType && c.Value == requirement.Permission))
        {
            context.Succeed(requirement);
            return;
        }

        // Check roles via PermissionChecker (Implementation B support)
        var userRoles = context.User.Claims
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .ToList();

        if (await _permissionChecker.HasPermissionAsync(userRoles, requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}
