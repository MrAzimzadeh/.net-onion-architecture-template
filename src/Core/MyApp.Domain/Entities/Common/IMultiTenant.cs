using System;

namespace MyApp.Domain.Entities.Common;

public interface IMultiTenant
{
    Guid? TenantId { get; set; }
    Tenant Tenant { get; set; }

}
