namespace MyApp.Application.Common.DTOs;

/// <summary>
/// DTO for account sign-in request with device information
/// </summary>
public class AuthSignInRequestDto
{
    public string EmailAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
}
