using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyApp.WebAPI.Middlewares;

namespace MyApp.WebAPI.Extension;

public static class ExceptionExtensions
{
    public static IServiceCollection AddExceptionConfig(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }

    public static IApplicationBuilder UseExceptionConfig(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
        return app;
    }
}
