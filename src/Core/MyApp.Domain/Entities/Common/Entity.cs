using System;

namespace MyApp.Domain.Entities.Common;

public abstract class Entity : IEntity
{
    public required Guid Id { get; set; }
}