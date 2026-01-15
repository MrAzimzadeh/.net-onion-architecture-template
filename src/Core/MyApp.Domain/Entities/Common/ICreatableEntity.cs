using System;

namespace MyApp.Domain.Entities.Common;

public interface ICreatableEntity
{
    Guid CreatedBy { get; set; }
    DateTime CreationAt { get; set; }
}