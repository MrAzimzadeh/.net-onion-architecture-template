namespace MyApp.Application.Common.DTOs;

/// <summary>
/// DTO for device session information
/// </summary>
public class DeviceSessionDto
{
    public Guid Id { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime LastUsedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsCurrentDevice { get; set; }
}
