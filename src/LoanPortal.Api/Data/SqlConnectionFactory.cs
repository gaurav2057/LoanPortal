using Microsoft.Data.SqlClient;

namespace LoanPortal.Api.Data;

/// <summary>
/// Creates SQL connections from the connection string in appsettings.json.
/// </summary>
public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
    string MasterConnectionString { get; }
    string AppConnectionString { get; }
}

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        AppConnectionString = _configuration.GetConnectionString("LoanPortal")
            ?? throw new InvalidOperationException("Connection string 'LoanPortal' is missing.");
        MasterConnectionString = _configuration.GetConnectionString("Master")
            ?? AppConnectionString.Replace("Database=LoanPortal", "Database=master");
    }

    public string MasterConnectionString { get; }
    public string AppConnectionString { get; }

    public SqlConnection CreateConnection() => new(AppConnectionString);
}
