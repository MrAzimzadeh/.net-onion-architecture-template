using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;
using MyApp.Domain.Entities.Common;

namespace MyApp.Persistence.Database;

/// <summary>
/// EF Core DbContext for migrations management
/// This is used ONLY for running migrations with 'dotnet ef migrations add'
/// All data access queries use Dapper instead
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Entity definitions for migrations
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<RolePolicy> RolePolicies { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<Policy> Policies { get; set; } = null!;
    public DbSet<Tenant> Tenants { get; set; } = null!;
    public DbSet<UserDeviceSession> UserDeviceSessions { get; set; } = null!;
    public DbSet<UserExternalLogin> UserExternalLogins { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
