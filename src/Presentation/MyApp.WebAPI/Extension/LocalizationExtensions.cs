using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace MyApp.WebAPI.Extension;

public static class LocalizationExtensions
{
    public static IServiceCollection AddLocalizationConfig(this IServiceCollection services)
    {
        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });

        return services;
    }

    public static IApplicationBuilder UseLocalizationConfig(this IApplicationBuilder app)
    {
        var supportedCultures = new[]
        {
            new CultureInfo("az-AZ"),
            new CultureInfo("ru-RU"),
            new CultureInfo("en-US"),
            new CultureInfo("tr-TR")
        };

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-US"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        return app;
    }
}
