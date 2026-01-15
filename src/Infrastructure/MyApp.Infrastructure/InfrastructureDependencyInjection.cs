using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyApp.Application.Common.Interfaces;
using MyApp.Infrastructure.Authentication;
using MyApp.Infrastructure.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using MyApp.Application.Security;
using MyApp.Infrastructure.Services;
using MyApp.Application.Common.DTOs;
using MyApp.Application.Common.Interfaces.Auth;
using MyApp.Application.Common.Interfaces.Mail;
using MyApp.Application.Common.DTOs.Mail;
using MyApp.Application.Common.Interfaces.Messaging;
using MyApp.Infrastructure.Messaging;
using MyApp.Infrastructure.BackgroundServices;

namespace MyApp.Infrastructure;

/// <summary>
/// Infrastructure layer dependency injection configuration
/// </summary>
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // JWT Authentication
        services.AddAuthentication("Bearer")
           .AddScheme<AuthenticationSchemeOptions, ProjectAuthHandler>("Bearer", null);

        // Register JWT Token Generator
        services.AddScoped<IJwtTokenService, JwtTokenService>();


        // Register Auth and Password Helpers
        services.AddScoped<IAuthHelper, AuthHelper>();
        services.AddScoped<IPasswordHandler, PasswordHandler>();

        // Register Authorization Services
        services.AddScoped<IPolicyChecker, PolicyChecker>();
        services.AddScoped<IAuthorizationHandler, PolicyAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();

        // Register Mail Service
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        services.AddTransient<IMailService, MailService>();

        // Register RabbitMQ Services
        services.AddSingleton<IMessageBus, RabbitMQBus>();
        services.AddHostedService<MailConsumer>();

        return services;
    }
}
