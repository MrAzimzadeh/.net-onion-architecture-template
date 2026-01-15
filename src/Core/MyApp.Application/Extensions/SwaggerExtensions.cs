using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace MyApp.Application.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Contact = new OpenApiContact()
                    {
                        Email = "mahammad@azimzada.com",
                        Name = "Mahammad Azimzada",
                        Url = new Uri("https://www.linkedin.com/in/mrazimzadeh/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://github.com/MrAzimzadeh/OnionArch-Template/blob/main/LICENSE"),
                    },
                    TermsOfService = new Uri("https://github.com/MrAzimzadeh/OnionArch-Template/blob/main/LICENSE"),
                    Title = "MyApp API",
                    Version = "v1",
                    Description = "MyApp Project",
                });
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {

                        new OpenApiSecurityScheme
                        {

                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey,
                            BearerFormat = "JWT",
                            Flows = new OpenApiOAuthFlows
                            {
                                AuthorizationCode = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = new Uri("https://example.com/oauth/authorize"),
                                    TokenUrl = new Uri("https://example.com/oauth/token"),
                                    Scopes = new Dictionary<string, string>
                                    {
                                        { "read", "Read scope" },
                                        { "write", "Write scope" }
                                    }
                                }
                            }
                            ,
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void UseCustomSwagger(this WebApplication app)
        {
            if (!app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DocExpansion(DocExpansion.None);
                    c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");

                });
            }
        }
    }
}