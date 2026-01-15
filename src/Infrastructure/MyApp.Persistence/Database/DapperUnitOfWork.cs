using System.Data;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Repositories.Read;
using MyApp.Application.Common.Interfaces.Repositories.Write;

namespace MyApp.Persistence.Database;

/// <summary>
/// Dapper-based Unit of Work implementation with CQRS repositories
/// </summary>
public class DapperUnitOfWork(
    IDbConnectionFactory connectionFactory,
    IUserReadRepository userReadRepository,
    IUserWriteRepository userWriteRepository,
    IRoleReadRepository roleReadRepository,
    IRoleWriteRepository roleWriteRepository) : IUnitOfWork
{
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;

    public IUserReadRepository UserReads { get; } = userReadRepository;
    public IUserWriteRepository UserWrites { get; } = userWriteRepository;
    public IRoleReadRepository RoleReads { get; } = roleReadRepository;
    public IRoleWriteRepository RoleWrites { get; } = roleWriteRepository;

    public IDbConnection Connection
    {
        get
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = connectionFactory.CreateConnection();
                _connection.Open();
            }
            return _connection;
        }
    }

    public IDbTransaction? Transaction => _transaction;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dapper doesn't track changes, so this is a no-op
        // Changes are committed via transaction
        return await Task.FromResult(0);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction already started.");

        _transaction = Connection.BeginTransaction();
        await Task.CompletedTask;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction to commit.");

        try
        {
            _transaction.Commit();
        }
        catch
        {
            _transaction.Rollback();
            throw;
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }

        await Task.CompletedTask;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction to rollback.");

        _transaction.Rollback();
        _transaction.Dispose();
        _transaction = null;

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }
            _disposed = true;
        }
    }
}
