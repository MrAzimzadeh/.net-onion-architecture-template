
using MyApp.Domain.Entities.Common;

namespace MyApp.Domain.Entities;

public class UserExternalLogin : Entity
{
    public Guid UserId { get; set; }
    public string Provider { get; set; } = null!;
    public string ExternalUserId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}
