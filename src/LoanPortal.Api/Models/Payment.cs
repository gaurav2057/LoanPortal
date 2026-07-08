namespace LoanPortal.Api.Models;

/// <summary>
/// Maps to dbo.Payments — money paid against a loan.
/// </summary>
public class Payment
{
    public int PaymentId { get; set; }
    public int LoanId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
}

public class CreatePaymentRequest
{
    public int LoanId { get; set; }
    public decimal Amount { get; set; }
}
