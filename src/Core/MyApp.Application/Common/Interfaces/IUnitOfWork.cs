using MyApp.Application.Common.Interfaces.Repositories.Read;
using MyApp.Application.Common.Interfaces.Repositories.Write;

namespace MyApp.Application.Common.Interfaces;

/// <summary>
/// Unit of Work pattern interface with CQRS Read/Write repositories
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // User repositories
    IUserReadRepository UserReads { get; }
    IUserWriteRepository UserWrites { get; }
    
    // Role repositories
    IRoleReadRepository RoleReads { get; }
    IRoleWriteRepository RoleWrites { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
