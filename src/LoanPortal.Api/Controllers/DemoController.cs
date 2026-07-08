using Dapper;
using LoanPortal.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace LoanPortal.Api.Controllers;

/// <summary>
/// Educational endpoints for SQL injection (Phase 3). Use only in local development.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DemoController : ControllerBase
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public DemoController(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>UNSAFE: concatenates email into SQL (do not use in production).</summary>
    [HttpGet("unsafe-search")]
    public async Task<IActionResult> UnsafeSearch([FromQuery] string email)
    {
        await using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT CustomerId, Name, Email FROM dbo.Customers WHERE Email = '" + email + "'";
        var rows = await connection.QueryAsync(sql);
        return Ok(rows);
    }

    /// <summary>SAFE: parameterized query via Dapper.</summary>
    [HttpGet("safe-search")]
    public async Task<IActionResult> SafeSearch([FromQuery] string email)
    {
        const string sql = """
            SELECT CustomerId, Name, Email
            FROM dbo.Customers
            WHERE Email = @Email;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql, new { Email = email });
        return Ok(rows);
    }
}
