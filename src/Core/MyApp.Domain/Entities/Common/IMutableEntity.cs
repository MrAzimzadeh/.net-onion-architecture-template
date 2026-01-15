#nullable enable
using System;

namespace MyApp.Domain.Entities.Common;

public interface IMutableEntity
{
    Guid? LastModifiedBy { get; set; }
    DateTime? LastModifiedAt { get; set; }
}