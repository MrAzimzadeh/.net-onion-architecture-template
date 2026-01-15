using Microsoft.AspNetCore.SignalR;
using MyApp.Application.Common.Interfaces.Hubs;

namespace MyApp.SignalR.Hubs;

/// <summary>
/// Specific SignalR Hub for role-related real-time communications
/// </summary>
public class RoleHub : Hub<IRoleHub>
{
}
