

using MyApp.Domain.Entities;
using MyApp.Domain.Entities.Common;

namespace MyApp.Domain.Entities;

public class UserRole : MutableEntity
{
    public required Guid UserId { get; set; }
    public User User { get; set; }
    public required Guid RoleId { get; set; }
    public Role Role { get; set; }
}