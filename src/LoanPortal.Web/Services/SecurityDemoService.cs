using Dapper;
using LoanPortal.Api.Data;
using LoanPortal.Api.Models;

namespace LoanPortal.Web.Services;

/// <summary>
/// Educational OWASP demo: unsafe string concat vs safe Dapper parameters.
/// Local development only — never use UnsafeSearch in production.
/// </summary>
public class SecurityDemoService
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public SecurityDemoService(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<Customer>> UnsafeSearchAsync(string email)
    {
        await using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT CustomerId, Name, Email FROM dbo.Customers WHERE Email = '" + email + "'";
        var rows = await connection.QueryAsync<Customer>(sql);
        return rows.ToList();
    }

    public async Task<IReadOnlyList<Customer>> SafeSearchAsync(string email)
    {
        const string sql = """
            SELECT CustomerId, Name, Email
            FROM dbo.Customers
            WHERE Email = @Email;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<Customer>(sql, new { Email = email });
        return rows.ToList();
    }
}
