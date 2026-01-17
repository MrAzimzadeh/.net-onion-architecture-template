using MyApp.Application;
using MyApp.Application.Extensions;
using MyApp.Infrastructure;
using MyApp.Persistence;
using MyApp.Persistence.Database.Migrations;
using Microsoft.EntityFrameworkCore;
using Serilog;
using MyApp.WebAPI.Extension;
using MyApp.SignalR;
using MyApp.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
builder.ConfigureSerilog();

// OpenTelemetry Configuration
builder.AddCustomOpenTelemetry();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();

// Add Application Layer services (MediatR, AutoMapper, FluentValidation)
builder.Services.AddApplicationServices();

// Add Persistence Layer services (Dapper, Repositories, UnitOfWork)
builder.Services.AddPersistenceServices(builder.Configuration);

// Add Infrastructure Layer services (JWT, Authentication, External Services)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add SignalR Layer services
builder.Services.AddSignalRServices();

// Add Rate Limiting
builder.Services.AddCustomRateLimiting();

// Add Localization
builder.Services.AddLocalizationConfig();

// Add Global Exception Handling
builder.Services.AddExceptionConfig();

// Add Health Checks
builder.Services.AddCustomHealthChecks(builder.Configuration);

// Add Storage (Choose your provider: Local, Azure, MinIO)
builder.Services.AddStorage(MyApp.Application.Common.Enums.StorageType.Local);

var app = builder.Build();

// Ensure database exists and run migrations on startup
await app.MigrateDatabaseAsync();

// Use Global Exception Handling Middleware
app.UseExceptionConfig();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.UseHttpsRedirection();
// Use Rate Limiting Middleware
app.UseCustomRateLimiting();

// Add Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Use Localization
app.UseLocalizationConfig();

// Use Health Checks
app.UseCustomHealthChecks();

app.MapControllers();
app.MapHub<UserHub>("/hubs/user");
app.MapHub<RoleHub>("/hubs/role");
app.UseAppCors();
app.Run();
