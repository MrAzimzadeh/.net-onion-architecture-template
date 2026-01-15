using Microsoft.AspNetCore.Authorization;

namespace MyApp.Infrastructure.Authentication;

public class PolicyAttribute : AuthorizeAttribute
{
    public PolicyAttribute(string pollicy) : base(policy: pollicy)
    {
    }
}
