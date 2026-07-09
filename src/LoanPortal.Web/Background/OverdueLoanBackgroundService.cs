using LoanPortal.Api.Repositories;
using LoanPortal.Web.Services;

namespace LoanPortal.Web.Background;

/// <summary>
/// Hosted service (BackgroundService) — checks for overdue loans on a timer.
/// Demonstrates ASP.NET Core Hosted Services for your company study sheet.
/// </summary>
public class OverdueLoanBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<OverdueLoanBackgroundService> _logger;

    public OverdueLoanBackgroundService(
        IServiceProvider services,
        ILogger<OverdueLoanBackgroundService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // First check after 10 seconds so demo works quickly after startup
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckOverdueLoansAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task CheckOverdueLoansAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var loanRepository = scope.ServiceProvider.GetRequiredService<ILoanRepository>();
        var alertStore = scope.ServiceProvider.GetRequiredService<OverdueLoanAlertStore>();

        var overdue = await loanRepository.GetOverdueAsync();
        var checkedAt = DateTime.UtcNow;

        var entries = overdue.Select(loan => new OverdueAlertEntry(
            loan.LoanId,
            loan.CustomerName,
            loan.Amount,
            loan.DueDate,
            checkedAt,
            $"Loan #{loan.LoanId} for {loan.CustomerName} is overdue (due {loan.DueDate:d}). Reminder email would be sent."))
            .ToList();

        alertStore.Update(entries);

        if (entries.Count == 0)
        {
            _logger.LogInformation("Overdue loan check: no overdue active loans.");
        }
        else
        {
            foreach (var entry in entries)
            {
                _logger.LogWarning("{Message}", entry.Message);
            }
        }
    }
}
