using Dapper;
using Microsoft.Data.SqlClient;

namespace LoanPortal.Api.Data;

/// <summary>
/// Runs SQL scripts on startup so the database and sample data exist before the API serves requests.
/// </summary>
public class DatabaseBootstrapper
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<DatabaseBootstrapper> _logger;

    public DatabaseBootstrapper(
        ISqlConnectionFactory connectionFactory,
        IWebHostEnvironment environment,
        ILogger<DatabaseBootstrapper> logger)
    {
        _connectionFactory = connectionFactory;
        _environment = environment;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await EnsureDatabaseExistsAsync(cancellationToken);
        await RunScriptAsync("01-create-tables.sql", cancellationToken);
        await RunScriptAsync("02-seed-data.sql", cancellationToken);
        _logger.LogInformation("Database bootstrap completed.");
    }

    private async Task EnsureDatabaseExistsAsync(CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionFactory.MasterConnectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = """
            IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'LoanPortal')
            BEGIN
                CREATE DATABASE LoanPortal;
            END
            """;

        await connection.ExecuteAsync(new CommandDefinition(sql, cancellationToken: cancellationToken));
    }

    private async Task RunScriptAsync(string fileName, CancellationToken cancellationToken)
    {
        var databaseDir = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, "..", "..", "database"));
        var scriptPath = Path.Combine(databaseDir, fileName);

        if (!File.Exists(scriptPath))
        {
            throw new FileNotFoundException($"SQL script not found: {scriptPath}");
        }

        var script = await File.ReadAllTextAsync(scriptPath, cancellationToken);
        var batches = script.Split("GO", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        foreach (var batch in batches)
        {
            if (string.IsNullOrWhiteSpace(batch))
            {
                continue;
            }

            await connection.ExecuteAsync(new CommandDefinition(batch, cancellationToken: cancellationToken));
        }

        _logger.LogInformation("Executed SQL script {Script}", fileName);
    }
}
