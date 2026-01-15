using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyApp.Infrastructure.Authentication;

/// <summary>
/// JWT token generator implementation
/// </summary>
public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{

    private readonly string _secretKey = configuration.GetValue<string>("JwtSettings:SecretKey") ?? string.Empty;
    private readonly int _expiredHours = configuration.GetValue<int>("JwtSettings:ExpiredHours");

    public string GenerateAccessToken(IList<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        return tokenHandler
            .WriteToken(new JwtSecurityToken(
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                expires: DateTime.UtcNow.AddHours(_expiredHours),
                claims: claims
            ));
    }

    public AuthenticateResult ValidateToken(string token, string schemaName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            LifetimeValidator = LifetimeValidator
        };

        var principal = tokenHandler.ValidateToken(token, tokenParameters, out _);
        var ticket = new AuthenticationTicket(principal, schemaName);

        return AuthenticateResult.Success(ticket);
    }

    private static bool LifetimeValidator(DateTime? notBefore,
        DateTime? expires,
        SecurityToken securityToken,
        TokenValidationParameters validationParameters)
    {
        return expires != null && expires > DateTime.UtcNow;
    }
}
