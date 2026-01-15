using Microsoft.AspNetCore.Authorization;

namespace MyApp.Infrastructure.Authentication;

public class PolicyRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PolicyRequirement(string permission)
    {
        Permission = permission;
    }
}
