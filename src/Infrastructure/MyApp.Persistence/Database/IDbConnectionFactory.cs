using System.Data;

namespace MyApp.Persistence.Database;

/// <summary>
/// Factory for creating database connections
/// </summary>
public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
    Task<IDbConnection> CreateConnectionAsync();
}
