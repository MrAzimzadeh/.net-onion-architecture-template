using Microsoft.AspNetCore.Http;

namespace MyApp.Application.Common.Interfaces.Auth;

/// <summary>
/// Helper interface for authentication-related operations
/// </summary>

public interface IAuthHelper
{
    /// <summary>
    /// Gets the current user ID from claims
    /// </summary>
    Guid GetUserId(HttpContext? httpContext);

    /// <summary>
    /// Gets the current user email from claims
    /// </summary>
    string? GetUserEmail(HttpContext? httpContext);

    /// <summary>
    /// Gets the current user's tenant ID from claims
    /// </summary>
    Guid? GetTenantId(HttpContext? httpContext);
    /// <summary>
    /// Gets the device ID from claims
    /// </summary>
    string? GetDeviceId(HttpContext? httpContext);

    /// <summary>
    /// Gets all user roles from claims
    /// </summary>
    List<string> GetUserRoles(HttpContext? httpContext);
}