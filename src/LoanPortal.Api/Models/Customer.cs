namespace LoanPortal.Api.Models;

/// <summary>
/// Maps to dbo.Customers — one row per borrower.
/// </summary>
public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
