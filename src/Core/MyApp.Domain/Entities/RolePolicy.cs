

using MyApp.Domain.Entities.Common;

namespace MyApp.Domain.Entities;

public class RolePolicy : MutableEntity
{
    public Guid RoleId { get; set; }
    public Guid PolicyId { get; set; }
    public bool IsActive { get; set; } = true;
    public virtual Role Role { get; set; }
    public virtual Policy Policy { get; set; }
}
