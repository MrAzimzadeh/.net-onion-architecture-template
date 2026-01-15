namespace MyApp.Application.Common.DTOs;

/// <summary>
/// DTO for changing password
/// </summary>
public class AuthChangePasswordDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
