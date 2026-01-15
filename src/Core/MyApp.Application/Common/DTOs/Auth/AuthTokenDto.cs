namespace MyApp.Application.Common.DTOs;

/// <summary>
/// DTO for access and refresh tokens
/// </summary>
public class AuthTokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiresAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
}
