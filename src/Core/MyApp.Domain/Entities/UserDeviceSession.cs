

using MyApp.Domain.Entities.Common;
namespace MyApp.Domain.Entities;

public class UserDeviceSession : MutableEntity
{
    public Guid UserId { get; set; }
    public string? DeviceId { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
    public string? DeviceType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUsedAt { get; set; }
    public string? DeviceName { get; set; }
    public bool IsActive { get; set; }

    // Navigation property
    public User User { get; set; }
}
