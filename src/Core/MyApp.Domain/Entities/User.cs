
using MyApp.Domain.Entities;
using MyApp.Domain.Entities.Common;

namespace MyApp.Domain.Entities;

public class User : MutableEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public bool IsActive { get; set; }
    public Guid? TenantId { get; set; }
    public Tenant Tenant { get; set; }

    public IList<UserRole> UserRoles { get; set; }
    public IList<UserDeviceSession> DeviceSessions { get; set; }
    public ICollection<UserExternalLogin> ExternalLogins { get; set; } = new List<UserExternalLogin>();
}