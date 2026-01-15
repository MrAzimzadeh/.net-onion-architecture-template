using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using MyApp.Application.Common.Interfaces;
using MyApp.Persistence.Database;
using MyApp.Persistence.Database.Migrations;
using MyApp.Persistence.Repositories;
using MyApp.Application.Common.Interfaces.Repositories.Read;
using MyApp.Application.Common.Interfaces.Repositories.Write;

namespace MyApp.Persistence;

/// <summary>
/// Persistence layer dependency injection configuration
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Register EF Core DbContext for migrations only
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Register Connection Factory and Dapper context
        services.AddSingleton<DapperContext>();
        services.AddSingleton<IDbConnectionFactory, PostgresConnectionFactory>();
        services.AddSingleton<DatabaseCreator>();

        // Register FluentMigrator runner for PostgreSQL
        services.AddLogging(lb => lb.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DependencyInjection).Assembly).For.Migrations());

        // Register Read Repositories (CQRS)
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IRoleReadRepository, RoleReadRepository>();

        // Register Write Repositories (CQRS)
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        services.AddScoped<IRoleWriteRepository, RoleWriteRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, DapperUnitOfWork>();

        // Register Background Worker for DB LISTEN/NOTIFY
        services.AddHostedService<MyApp.Persistence.Workers.PostgresNotificationWorker>();

        return services;
    }
}
