using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using System;

namespace MyApp.WebAPI.Extension;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck("Self", () => HealthCheckResult.Healthy(), ["api"])
            .AddNpgSql(
                connectionString: configuration.GetConnectionString("DefaultConnection")!,
                name: "PostgreSQL",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["db", "sql", "postgresql"])
            .AddRabbitMQ(
                sp =>
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = configuration["RabbitMQ:HostName"] ?? "localhost",
                        UserName = configuration["RabbitMQ:UserName"] ?? "guest",
                        Password = configuration["RabbitMQ:Password"] ?? "guest"
                    };
                    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
                },
                name: "RabbitMQ",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["messaging", "rabbitmq"]);

        return services;
    }

    public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app)
    {
        // General health check (all checks)
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Liveness probe (only checks if API is up)
        app.UseHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("api"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Readiness probe (checks if all dependencies are ready)
        app.UseHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}
