using Microsoft.AspNetCore.SignalR;
using MyApp.Application.Common.Interfaces.Hubs;

namespace MyApp.SignalR.Hubs;

/// <summary>
/// Specific SignalR Hub for user-related real-time communications
/// </summary>
public class UserHub : Hub<IUserHub>
{
}
