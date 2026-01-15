using Dapper;
using MyApp.Application.Common.Interfaces.Repositories.Read;
using MyApp.Persistence.Database;

namespace MyApp.Persistence.Repositories.Base;

/// <summary>
/// Generic read repository implementation with Dapper
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public class ReadRepository<T>(IDbConnectionFactory connectionFactory, string tableName) : IReadRepository<T> where T : class
{
    protected readonly IDbConnectionFactory _connectionFactory = connectionFactory;
    protected readonly string _tableName = tableName;

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_tableName}";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<T>(sql);
    }

    public virtual async Task<IEnumerable<T>> GetWhereAsync(string whereClause, object parameters, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_tableName} WHERE {whereClause}";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<T>(sql, parameters);
    }

    public virtual async Task<T?> GetSingleAsync(string whereClause, object parameters, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_tableName} WHERE {whereClause} LIMIT 1";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_tableName} WHERE \"Id\" = @Id";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT COUNT(*) FROM {_tableName}";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<int>(sql);
    }

    public virtual async Task<int> CountWhereAsync(string whereClause, object parameters, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT COUNT(*) FROM {_tableName} WHERE {whereClause}";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<int>(sql, parameters);
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT COUNT(1) FROM {_tableName} WHERE \"Id\" = @Id";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
        return count > 0;
    }
}
