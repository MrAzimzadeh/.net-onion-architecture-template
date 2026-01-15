using Dapper;
using MyApp.Application.Common.Interfaces.Repositories.Write;
using MyApp.Persistence.Database;

namespace MyApp.Persistence.Repositories.Base;

/// <summary>
/// Generic write repository implementation with Dapper
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public class WriteRepository<T>(IDbConnectionFactory connectionFactory, string tableName) : IWriteRepository<T> where T : class
{
    protected readonly IDbConnectionFactory _connectionFactory = connectionFactory;
    protected readonly string _tableName = tableName;

    public virtual async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        // This needs to be overridden in specific repositories with actual INSERT SQL
        throw new NotImplementedException($"AddAsync must be implemented in derived repository for {_tableName}");
    }

    public virtual async Task<bool> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            foreach (var entity in entities)
            {
                await AddAsync(entity, cancellationToken);
            }
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public virtual async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        // This needs to be overridden in specific repositories with actual UPDATE SQL
        throw new NotImplementedException($"UpdateAsync must be implemented in derived repository for {_tableName}");
    }

    public virtual async Task<bool> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity, cancellationToken);
            }
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public virtual async Task<bool> RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        // This needs to be overridden in specific repositories
        throw new NotImplementedException($"RemoveAsync must be implemented in derived repository for {_tableName}");
    }

    public virtual async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sql = $"DELETE FROM {_tableName} WHERE \"Id\" = @Id";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }

    public virtual async Task<bool> RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            foreach (var entity in entities)
            {
                await RemoveAsync(entity, cancellationToken);
            }
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public virtual async Task<bool> RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var sql = $"DELETE FROM {_tableName} WHERE \"Id\" IN @Ids";
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var rowsAffected = await connection.ExecuteAsync(sql, new { Ids = ids });
        return rowsAffected > 0;
    }
}
