using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace MyApp.Application.Common.Interfaces.Auth;

/// <summary>
/// JWT token generator interface
/// </summary>
public interface IJwtTokenService
{
    string GenerateAccessToken(IList<Claim> claims);
    AuthenticateResult ValidateToken(string token, string schemaName);
}
