namespace MyApp.Application.Common.DTOs;

/// <summary>
/// DTO for external authentication provider login
/// </summary>
public class ExternalLoginDto
{
    public string Provider { get; set; } = string.Empty; // "Google", "Facebook", "Microsoft", etc.
    public string ProviderKey { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
}
