using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Persistence.Database;
using Npgsql;

namespace MyApp.Persistence.Database.Migrations;

/// <summary>
/// Extension methods for database migrations
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// Applies EF Core migrations and ensures database exists
    /// </summary>
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
        
        try
        {
            // Apply EF Core migrations
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully.");
        }
        catch (PostgresException pgEx) when (pgEx.SqlState == "42P07") // 42P07 = table already exists
        {
            logger.LogWarning("Database schema already exists (table already exists). Migration skipped. Details: {Message}", 
                pgEx.MessageText);
            // Schema already exists, continue anyway
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }
}
