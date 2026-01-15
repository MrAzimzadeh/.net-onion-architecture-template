using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Application.Common.Interfaces.Triggers;
using MyApp.Application.Triggers;

namespace MyApp.Application;

/// <summary>
/// Application layer dependency injection configuration
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // Register AutoMapper
        services.AddAutoMapper(assembly);

        // Register FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // Register Entity Triggers
        services.AddScoped<IEntityTrigger, UserTrigger>();
        services.AddScoped<IEntityTrigger, RoleTrigger>();

        // Register HttpContextAccessor
        services.AddHttpContextAccessor();

        // Register Localization Service
        services.AddScoped<MyApp.Application.Common.Interfaces.ILocalizationService, MyApp.Application.Common.Services.LocalizationService>();

        return services;
    }
}
