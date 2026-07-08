namespace LoanPortal.Api.Models;

/// <summary>
/// Maps to dbo.Loans — amount, status, and due date for a customer.
/// </summary>
public class Loan
{
    public int LoanId { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Loan plus customer name — used when listing loans with a JOIN.
/// </summary>
public class LoanSummary
{
    public int LoanId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalPaid { get; set; }
}

public class LoanDetail
{
    public Loan Loan { get; set; } = new();
    public Customer Customer { get; set; } = new();
    public IReadOnlyList<Payment> Payments { get; set; } = Array.Empty<Payment>();
}

public class CreateLoanRequest
{
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
}
