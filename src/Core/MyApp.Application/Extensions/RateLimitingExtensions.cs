using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;

namespace MyApp.Application.Extensions
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10, // 10 request
                            Window = TimeSpan.FromSeconds(10), // 10 saniyəlik pəncərə
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 2
                        }
                    ));
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });
            return services;
        }

        public static IApplicationBuilder UseCustomRateLimiting(this IApplicationBuilder app)
        {
            app.UseRateLimiter();
            return app;
        }
    }
}
