using Dapper;
using LoanPortal.Api.Data;
using LoanPortal.Api.Models;

namespace LoanPortal.Api.Repositories;

public interface ILoanRepository
{
    Task<IReadOnlyList<LoanSummary>> GetAllAsync();
    Task<IReadOnlyList<LoanSummary>> GetOverdueAsync();
    Task<LoanDetail?> GetDetailAsync(int loanId);
    Task<int> CreateAsync(CreateLoanRequest request);
    Task<bool> ApproveAsync(int loanId);
}

public class LoanRepository : ILoanRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public LoanRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<LoanSummary>> GetAllAsync()
    {
        const string sql = """
            SELECT
                l.LoanId,
                l.CustomerId,
                c.Name AS CustomerName,
                l.Amount,
                l.Status,
                l.DueDate,
                l.CreatedAt,
                ISNULL(SUM(p.Amount), 0) AS TotalPaid
            FROM dbo.Loans l
            INNER JOIN dbo.Customers c ON c.CustomerId = l.CustomerId
            LEFT JOIN dbo.Payments p ON p.LoanId = l.LoanId
            GROUP BY l.LoanId, l.CustomerId, c.Name, l.Amount, l.Status, l.DueDate, l.CreatedAt
            ORDER BY l.LoanId;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<LoanSummary>(sql);
        return rows.ToList();
    }

    public async Task<IReadOnlyList<LoanSummary>> GetOverdueAsync()
    {
        const string sql = """
            SELECT
                l.LoanId,
                l.CustomerId,
                c.Name AS CustomerName,
                l.Amount,
                l.Status,
                l.DueDate,
                l.CreatedAt,
                ISNULL(SUM(p.Amount), 0) AS TotalPaid
            FROM dbo.Loans l
            INNER JOIN dbo.Customers c ON c.CustomerId = l.CustomerId
            LEFT JOIN dbo.Payments p ON p.LoanId = l.LoanId
            WHERE l.Status = N'Active' AND l.DueDate < CAST(GETDATE() AS DATE)
            GROUP BY l.LoanId, l.CustomerId, c.Name, l.Amount, l.Status, l.DueDate, l.CreatedAt
            ORDER BY l.DueDate;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<LoanSummary>(sql);
        return rows.ToList();
    }

    public async Task<LoanDetail?> GetDetailAsync(int loanId)
    {
        const string loanSql = """
            SELECT LoanId, CustomerId, Amount, Status, DueDate, CreatedAt
            FROM dbo.Loans
            WHERE LoanId = @LoanId;
            """;

        const string customerSql = """
            SELECT CustomerId, Name, Email
            FROM dbo.Customers
            WHERE CustomerId = @CustomerId;
            """;

        const string paymentsSql = """
            SELECT PaymentId, LoanId, Amount, PaidAt
            FROM dbo.Payments
            WHERE LoanId = @LoanId
            ORDER BY PaidAt;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        var loan = await connection.QuerySingleOrDefaultAsync<Loan>(loanSql, new { LoanId = loanId });
        if (loan is null)
        {
            return null;
        }

        var customer = await connection.QuerySingleAsync<Customer>(
            customerSql,
            new { loan.CustomerId });

        var payments = await connection.QueryAsync<Payment>(paymentsSql, new { LoanId = loanId });

        return new LoanDetail
        {
            Loan = loan,
            Customer = customer,
            Payments = payments.ToList()
        };
    }

    public async Task<int> CreateAsync(CreateLoanRequest request)
    {
        const string sql = """
            INSERT INTO dbo.Loans (CustomerId, Amount, Status, DueDate)
            OUTPUT INSERTED.LoanId
            VALUES (@CustomerId, @Amount, N'Pending', @DueDate);
            """;

        await using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, request);
    }

    public async Task<bool> ApproveAsync(int loanId)
    {
        const string sql = """
            UPDATE dbo.Loans
            SET Status = N'Active'
            WHERE LoanId = @LoanId AND Status = N'Pending';
            """;

        await using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, new { LoanId = loanId });
        return rows == 1;
    }
}
