using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApp.Application.Common.Interfaces.Auth;

public interface IPolicyChecker
{
    Task<bool> HasPermissionAsync(List<string> roles, string permission);
}
