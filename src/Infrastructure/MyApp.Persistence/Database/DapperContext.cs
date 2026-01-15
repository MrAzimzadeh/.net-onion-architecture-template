using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace MyApp.Persistence.Database;

/// <summary>
/// Simple Dapper context for creating PostgreSQL connections
/// </summary>
public class DapperContext
{
    private readonly IConfiguration _configuration;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string DefaultConnectionString =>
        _configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    
    public string MasterConnectionString =>
        _configuration.GetConnectionString("MasterConnection")
        ?? DefaultConnectionString;

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(DefaultConnectionString);
    }

    public IDbConnection CreateMasterConnection()
    {
        return new NpgsqlConnection(MasterConnectionString);
    }
}
