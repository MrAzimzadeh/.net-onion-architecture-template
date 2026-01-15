using System;

namespace MyApp.Domain.Entities.Common;

public abstract class MutableEntity : Entity, ICreatableEntity, IMutableEntity
{
    public required Guid CreatedBy { get; set; }
    public DateTime CreationAt { get; set; }

    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}