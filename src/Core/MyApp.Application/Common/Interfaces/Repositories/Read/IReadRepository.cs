using System.Linq.Expressions;

namespace MyApp.Application.Common.Interfaces.Repositories.Read;

/// <summary>
/// Generic read repository interface for CQRS pattern
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IReadRepository<T> : IRepository<T> where T : class
{
    // Sync methods
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetWhereAsync(string whereClause, object parameters, CancellationToken cancellationToken = default);
    Task<T?> GetSingleAsync(string whereClause, object parameters, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountWhereAsync(string whereClause, object parameters, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
