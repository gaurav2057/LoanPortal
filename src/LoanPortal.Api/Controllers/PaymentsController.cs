using LoanPortal.Api.Models;
using LoanPortal.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LoanPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentRepository _payments;

    public PaymentsController(IPaymentRepository payments)
    {
        _payments = payments;
    }

    /// <summary>Record a payment against a loan.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
    {
        if (request.Amount <= 0)
        {
            return BadRequest("Amount must be greater than zero.");
        }

        var paymentId = await _payments.CreateAsync(request);
        if (paymentId == 0)
        {
            return NotFound("Loan not found.");
        }

        return Created(string.Empty, new { paymentId, request.LoanId, request.Amount });
    }
}
