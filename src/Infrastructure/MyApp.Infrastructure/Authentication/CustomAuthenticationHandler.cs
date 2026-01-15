using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;

namespace MyApp.Infrastructure.Authentication;

public class ProjectAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IJwtTokenService _jwtService;

    public ProjectAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IJwtTokenService jwtService) : base(options, logger, encoder)
    {
        _jwtService = jwtService;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            string? token = null;

            // 1) Try Authorization header first
            if (Request.Headers.ContainsKey("Authorization"))
            {
                var header = Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrWhiteSpace(header) && header.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
                {
                    token = header.Substring("bearer".Length).Trim();
                }
            }

            // 2) If not present, allow SignalR to pass token via query string for hub connections
            if (string.IsNullOrEmpty(token))
            {
                var accessToken = Request.Query["access_token"].ToString();
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    // Optionally restrict to hub path to avoid enabling query tokens globally
                    // Using generic check or specific if needed.
                    if (Request.Path.HasValue && (Request.Path.Value!.Contains("Hub") || Request.Path.Value.Contains("hub")))
                    {
                        token = accessToken;
                    }
                }
            }

            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
            }

            // Using "Bearer" as schema name or whatever validation logic requires
            return Task.FromResult(_jwtService.ValidateToken(token, Scheme.Name));
        }
        catch (Exception ex)
        {
            Logger.LogInformation($"Error:{ex.Message}: {ex.StackTrace} ");
            return Task.FromResult(AuthenticateResult.Fail(ex.Message));
        }
    }
}
