using Microsoft.Extensions.DependencyInjection;
using MyApp.Application.Common.Interfaces.Notifications;
using MyApp.SignalR.Services;

namespace MyApp.SignalR;

public static class DependencyInjection
{
    public static IServiceCollection AddSignalRServices(this IServiceCollection services)
    {
        services.AddSignalR();

        // Register Infrastructure Implementations of Application Interfaces
        services.AddSingleton<IUserNotificationService, SignalRUserNotificationService>();
        services.AddSingleton<IRoleNotificationService, SignalRRoleNotificationService>();

        return services;
    }
}
