using Dapper;
using LoanPortal.Api.Data;
using LoanPortal.Api.Models;

namespace LoanPortal.Api.Repositories;

public interface IPaymentRepository
{
    Task<int> CreateAsync(CreatePaymentRequest request);
}

public class PaymentRepository : IPaymentRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public PaymentRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CreateAsync(CreatePaymentRequest request)
    {
        const string loanExistsSql = "SELECT COUNT(1) FROM dbo.Loans WHERE LoanId = @LoanId;";
        const string insertSql = """
            INSERT INTO dbo.Payments (LoanId, Amount)
            OUTPUT INSERTED.PaymentId
            VALUES (@LoanId, @Amount);
            """;

        await using var connection = _connectionFactory.CreateConnection();
        var exists = await connection.ExecuteScalarAsync<int>(loanExistsSql, new { request.LoanId });
        if (exists == 0)
        {
            return 0;
        }

        return await connection.ExecuteScalarAsync<int>(insertSql, request);
    }
}
