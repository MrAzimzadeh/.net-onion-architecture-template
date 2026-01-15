using System;

namespace MyApp.Domain.Entities.Common;

public interface IEntity
{
    Guid Id { get; set; }
}