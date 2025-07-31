using Microsoft.AspNetCore.Mvc;
using NannyServices.Application.DTOs;
using NannyServices.Application.Services;
using NannyServices.Domain.Enums;

namespace NannyServices.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(OrderService orderService) : ControllerBase
{
    /// <summary>
    /// Get all orders with pagination
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Invalid pagination parameters");
        }

        var result = await orderService.GetPagedAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await orderService.GetByIdAsync(id);
        return order != null ? Ok(order) : NotFound();
    }

    /// <summary>
    /// Get orders by customer ID
    /// </summary>
    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetOrdersByCustomer(Guid customerId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Invalid pagination parameters");
        }

        var result = await orderService.GetPagedByCustomerAsync(customerId, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Get orders by status
    /// </summary>
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetOrdersByStatus(OrderStatus status)
    {
        var orders = await orderService.GetByStatusAsync(status);
        return Ok(orders);
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        try
        {
            var order = await orderService.CreateAsync(createOrderDto);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update order status
    /// </summary>
    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusDto updateStatusDto)
    {
        try
        {
            var order = await orderService.UpdateStatusAsync(id, updateStatusDto);
            return order != null ? Ok(order) : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Add order line to order
    /// </summary>
    [HttpPost("{id:guid}/order-lines")]
    public async Task<IActionResult> AddOrderLine(Guid id, [FromBody] AddOrderLineDto addOrderLineDto)
    {
        try
        {
            var order = await orderService.AddOrderLineAsync(id, addOrderLineDto);
            return order != null ? Ok(order) : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update order line quantity
    /// </summary>
    [HttpPut("{id:guid}/order-lines")]
    public async Task<IActionResult> UpdateOrderLine(Guid id, [FromBody] UpdateOrderLineDto updateOrderLineDto)
    {
        try
        {
            var order = await orderService.UpdateOrderLineAsync(id, updateOrderLineDto);
            return order != null ? Ok(order) : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Remove order line from order
    /// </summary>
    [HttpDelete("{id:guid}/order-lines/{orderLineId:guid}")]
    public async Task<IActionResult> RemoveOrderLine(Guid id, Guid orderLineId)
    {
        try
        {
            var order = await orderService.RemoveOrderLineAsync(id, orderLineId);
            return order != null ? Ok(order) : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete an order (only allowed for Created orders)
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        try
        {
            var deleted = await orderService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}