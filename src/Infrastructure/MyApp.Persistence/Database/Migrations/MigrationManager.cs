using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyApp.Persistence.Database.Migrations;

/// <summary>
/// Runs database creation and migrations on application start.
/// </summary>
public static class MigrationManager
{
    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("DatabaseMigrations");
        var databaseCreator = services.GetRequiredService<DatabaseCreator>();
        var migrationRunner = services.GetRequiredService<IMigrationRunner>();

        try
        {
            logger.LogInformation("Starting database migrations...");
            await databaseCreator.EnsureDatabaseCreatedAsync();
            migrationRunner.ListMigrations();
            migrationRunner.MigrateUp();
            logger.LogInformation("Database migrations completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error running database migrations");
            throw;
        }

        return host;
    }
}
