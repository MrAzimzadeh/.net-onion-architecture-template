using Microsoft.AspNetCore.Http;
using MyApp.Application.Common.Interfaces.Auth;
using System.Security.Claims;

namespace MyApp.Infrastructure.Helpers;

/// <summary>
/// Helper for extracting authentication information from HttpContext
/// </summary>
public  class AuthHelper : IAuthHelper
{
    /// <summary>
    /// Gets the current user ID from claims
    /// </summary>
    public  Guid GetUserId(HttpContext? httpContext)
    {
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
            throw new UnauthorizedAccessException("User is not authenticated");

        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("Invalid user ID in token");

        return userId;
    }

    /// <summary>
    /// Gets the current user email from claims
    /// </summary>
    public  string? GetUserEmail(HttpContext? httpContext)
    {
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
            return null;

        return httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
    }

    /// <summary>
    /// Gets the current user's tenant ID from claims
    /// </summary>
    public  Guid? GetTenantId(HttpContext? httpContext)
    {
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
            return null;

        var tenantIdClaim = httpContext.User.FindFirst("tenant_id")?.Value;
        
        if (string.IsNullOrEmpty(tenantIdClaim))
            return null;

        return Guid.TryParse(tenantIdClaim, out var tenantId) ? tenantId : null;
    }

    /// <summary>
    /// Gets the device ID from claims
    /// </summary>
    public  string? GetDeviceId(HttpContext? httpContext)
    {
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
            return null;

        return httpContext.User.FindFirst("device_id")?.Value;
    }

    /// <summary>
    /// Gets all user roles from claims
    /// </summary>
    public  List<string> GetUserRoles(HttpContext? httpContext)
    {
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
            return new List<string>();

        return httpContext.User.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
    }
}
