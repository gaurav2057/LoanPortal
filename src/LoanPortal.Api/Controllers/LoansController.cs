using LoanPortal.Api.Models;
using LoanPortal.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LoanPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly ILoanRepository _loans;
    private readonly ICustomerRepository _customers;

    public LoansController(ILoanRepository loans, ICustomerRepository customers)
    {
        _loans = loans;
        _customers = customers;
    }

    /// <summary>List all loans with customer name and total paid (uses SQL JOIN).</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var loans = await _loans.GetAllAsync();
        return Ok(loans);
    }

    /// <summary>Get loan detail including customer and payment history.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var detail = await _loans.GetDetailAsync(id);
        return detail is null ? NotFound() : Ok(detail);
    }

    /// <summary>Create a new loan in Pending status.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLoanRequest request)
    {
        if (request.Amount <= 0)
        {
            return BadRequest("Amount must be greater than zero.");
        }

        var customer = await _customers.GetByIdAsync(request.CustomerId);
        if (customer is null)
        {
            return BadRequest("Customer does not exist.");
        }

        var loanId = await _loans.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = loanId }, new { loanId });
    }

    /// <summary>Approve a pending loan (officer action in later phases).</summary>
    [HttpPost("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var approved = await _loans.ApproveAsync(id);
        if (!approved)
        {
            return BadRequest("Loan not found or not in Pending status.");
        }

        return Ok(new { loanId = id, status = "Active" });
    }
}
