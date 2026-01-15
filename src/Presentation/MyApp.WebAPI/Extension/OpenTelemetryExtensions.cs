using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Npgsql;

namespace MyApp.WebAPI.Extension;

public static class OpenTelemetryExtensions
{
    public static void AddCustomOpenTelemetry(this WebApplicationBuilder builder)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService("MyApp.WebAPI")
            .AddAttributes(new Dictionary<string, object>
            {
                ["deployment.environment"] = builder.Environment.EnvironmentName,
                ["project.type"] = "OnionArchitecture"
            });

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;
                })
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation(options =>
                {
                    options.SetDbStatementForText = true; // EF Core 
                })
                .AddNpgsql() // Dapper/Npgsql 
                .AddSource("MyApp.WebAPI", "MyApp.Application", "RabbitMQ.Client")
                .AddOtlpExporter(options =>
                {
                    // Jaeger OTLP endpoint (Default: 4317)
                    options.Endpoint = new Uri("http://localhost:4317");
                }))
            .WithMetrics(metrics => metrics
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation() // CPU, RAM, GC, ThreadPool 
                .AddProcessInstrumentation() // Process level memory, CPU
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri("http://localhost:4317");
                }));
    }
}
