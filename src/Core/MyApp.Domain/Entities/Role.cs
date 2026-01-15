
using MyApp.Domain.Entities.Common;

namespace MyApp.Domain.Entities;

public class Role : MutableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public IList<UserRole> UserRoles { get; set; }
    public IList<RolePolicy> RolePolicies { get; set; }
}
