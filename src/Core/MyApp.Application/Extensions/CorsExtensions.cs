using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace MyApp.Application.Extensions
{
    public static class CorsExtensions
    {
        public static void UseAppCors(this WebApplication app)
        {
            IConfiguration configuration = app.Configuration;
            string? origins = configuration.GetValue<string>(key: "Cors:Origins");

            app.UseCors(builder =>
            {
                builder
                    .WithOrigins(origins != null
                        ? origins.Split(";")
                        : Array.Empty<string>())
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        }
    }
}