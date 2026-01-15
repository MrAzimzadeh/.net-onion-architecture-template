using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace MyApp.Persistence.Database;

/// <summary>
/// Creates target PostgreSQL database if it does not exist.
/// </summary>
public class DatabaseCreator
{
    private readonly DapperContext _context;
    private readonly ILogger<DatabaseCreator> _logger;

    public DatabaseCreator(DapperContext context, ILogger<DatabaseCreator> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task EnsureDatabaseCreatedAsync(CancellationToken cancellationToken = default)
    {
        await using var masterConnection = (NpgsqlConnection)_context.CreateMasterConnection();
        await masterConnection.OpenAsync(cancellationToken);

        var targetBuilder = new NpgsqlConnectionStringBuilder(_context.DefaultConnectionString);
        var targetDatabase = targetBuilder.Database;

        const string checkSql = "SELECT 1 FROM pg_database WHERE datname = @name";
        var exists = await masterConnection.ExecuteScalarAsync<int?>(new CommandDefinition(checkSql, new { name = targetDatabase }, cancellationToken: cancellationToken));
        if (exists.HasValue)
        {
            _logger.LogInformation("Database {Database} already exists", targetDatabase);
            return;
        }

        var createSql = $"CREATE DATABASE \"{targetDatabase}\"";
        await masterConnection.ExecuteAsync(new CommandDefinition(createSql, cancellationToken: cancellationToken));
        _logger.LogInformation("Database {Database} created", targetDatabase);
    }
}
