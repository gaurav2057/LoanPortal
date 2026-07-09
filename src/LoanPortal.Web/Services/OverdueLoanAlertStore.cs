namespace LoanPortal.Web.Services;

/// <summary>
/// In-memory store updated by the background worker so officers can see recent overdue checks.
/// </summary>
public class OverdueLoanAlertStore
{
    private readonly object _lock = new();
    private List<OverdueAlertEntry> _entries = new();

    public void Update(IReadOnlyList<OverdueAlertEntry> entries)
    {
        lock (_lock)
        {
            _entries = entries.ToList();
        }
    }

    public IReadOnlyList<OverdueAlertEntry> GetLatest()
    {
        lock (_lock)
        {
            return _entries.ToList();
        }
    }
}

public record OverdueAlertEntry(
    int LoanId,
    string CustomerName,
    decimal Amount,
    DateTime DueDate,
    DateTime CheckedAtUtc,
    string Message);
