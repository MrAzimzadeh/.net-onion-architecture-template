using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;


namespace MyApp.Infrastructure.Authentication;

public class DynamicAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public DynamicAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy != null)
        {
            return policy;
        }

        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PolicyRequirement(policyName))
            .Build();
    }
}
