namespace MyApp.Application.Common.Interfaces.Repositories.Write;

/// <summary>
/// Generic write repository interface for CQRS pattern
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IWriteRepository<T> : IRepository<T> where T : class
{
    // Async methods
    Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task<bool> RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}
