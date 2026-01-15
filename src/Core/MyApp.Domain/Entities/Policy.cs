using MyApp.Domain.Entities.Common;

namespace MyApp.Domain.Entities;


public class Policy : MutableEntity
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Group { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}
