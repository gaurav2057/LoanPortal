using LoanPortal.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LoanPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customers;

    public CustomersController(ICustomerRepository customers)
    {
        _customers = customers;
    }

    /// <summary>List all customers.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customers.GetAllAsync();
        return Ok(customers);
    }

    /// <summary>Get one customer by id.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customers.GetByIdAsync(id);
        return customer is null ? NotFound() : Ok(customer);
    }
}
