using Microsoft.AspNetCore.Mvc;
using NannyServices.Application.DTOs;
using NannyServices.Application.Services;

namespace NannyServices.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(CustomerService customerService) : ControllerBase
{
    /// <summary>
    /// Get all customers with pagination
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCustomers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Invalid pagination parameters");
        }

        var result = await customerService.GetPagedAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        var customer = await customerService.GetByIdAsync(id);
        return customer != null ? Ok(customer) : NotFound();
    }

    /// <summary>
    /// Get customer by ID with their orders
    /// </summary>
    [HttpGet("{id:guid}/with-orders")]
    public async Task<IActionResult> GetCustomerWithOrders(Guid id)
    {
        var customer = await customerService.GetByIdWithOrdersAsync(id);
        return customer != null ? Ok(customer) : NotFound();
    }

    /// <summary>
    /// Search customers by name
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> SearchCustomers([FromQuery] string? searchTerm)
    {
        var customers = await customerService.SearchByNameAsync(searchTerm ?? string.Empty);
        return Ok(customers);
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
    {
        try
        {
            var customer = await customerService.CreateAsync(createCustomerDto);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerDto updateCustomerDto)
    {
        try
        {
            var customer = await customerService.UpdateAsync(id, updateCustomerDto);
            return customer != null ? Ok(customer) : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        var deleted = await customerService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Get customer report for a date range
    /// </summary>
    [HttpGet("{id:guid}/reports")]
    public async Task<IActionResult> GetCustomerReport(
        Guid id,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        if (startDate >= endDate)
        {
            return BadRequest("Start date must be before end date");
        }

        var report = await customerService.GetReportAsync(id, startDate, endDate);
        return report != null ? Ok(report) : NotFound();
    }

    /// <summary>
    /// Get customer report for the current week
    /// </summary>
    [HttpGet("{id:guid}/reports/week")]
    public async Task<IActionResult> GetCustomerWeeklyReport(Guid id)
    {
        var today = DateTime.Today;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(7);

        var report = await customerService.GetReportAsync(id, startOfWeek, endOfWeek);
        return report != null ? Ok(report) : NotFound();
    }

    /// <summary>
    /// Get customer report for the current month
    /// </summary>
    [HttpGet("{id:guid}/reports/month")]
    public async Task<IActionResult> GetCustomerMonthlyReport(Guid id)
    {
        var today = DateTime.Today;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1);

        var report = await customerService.GetReportAsync(id, startOfMonth, endOfMonth);
        return report != null ? Ok(report) : NotFound();
    }
}