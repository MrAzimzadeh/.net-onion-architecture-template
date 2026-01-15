using System;

namespace MyApp.Domain.Entities.Common;

public abstract class CreatableEntity : Entity, ICreatableEntity
{
    public required Guid CreatedBy { get; set; }
    public DateTime CreationAt { get; set; }
}