using Dapper;
using LoanPortal.Api.Data;
using LoanPortal.Api.Models;

namespace LoanPortal.Api.Repositories;

public interface ICustomerRepository
{
    Task<IReadOnlyList<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int customerId);
}

public class CustomerRepository : ICustomerRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CustomerRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<Customer>> GetAllAsync()
    {
        const string sql = """
            SELECT CustomerId, Name, Email
            FROM dbo.Customers
            ORDER BY Name;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<Customer>(sql);
        return rows.ToList();
    }

    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        const string sql = """
            SELECT CustomerId, Name, Email
            FROM dbo.Customers
            WHERE CustomerId = @CustomerId;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { CustomerId = customerId });
    }
}
