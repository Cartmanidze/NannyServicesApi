using MediatR;
using Microsoft.AspNetCore.Mvc;
using NannyServices.Application.Orders.Commands;
using NannyServices.Application.Orders.Queries;
using NannyServices.Application.DTOs;
using NannyServices.Domain.Enums;

namespace NannyServices.Api.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders")
            .WithTags("Orders")
            .WithOpenApi();

        // Queries
        group.MapGet("/", GetOrders)
            .WithName("get-orders")
            .WithSummary("Get all orders with pagination");

        group.MapGet("/{id:guid}", GetOrder)
            .WithName("get-order-by-id")
            .WithSummary("Get order by ID");

        group.MapGet("/customer/{customerId:guid}", GetOrdersByCustomer)
            .WithName("get-orders-by-customer")
            .WithSummary("Get orders by customer ID");

        group.MapGet("/status/{status}", GetOrdersByStatus)
            .WithName("get-orders-by-status")
            .WithSummary("Get orders by status");

        // Commands
        group.MapPost("/", CreateOrder)
            .WithName("create-order")
            .WithSummary("Create a new order");

        group.MapPut("/{id:guid}/status", UpdateOrderStatus)
            .WithName("update-order-status")
            .WithSummary("Update order status");

        group.MapPost("/{id:guid}/order-lines", AddOrderLine)
            .WithName("add-order-line")
            .WithSummary("Add order line to order");

        group.MapPut("/{id:guid}/order-lines", UpdateOrderLine)
            .WithName("update-order-line")
            .WithSummary("Update order line quantity");

        group.MapDelete("/{id:guid}/order-lines/{orderLineId:guid}", RemoveOrderLine)
            .WithName("remove-order-line")
            .WithSummary("Remove order line from order");

        group.MapDelete("/{id:guid}", DeleteOrder)
            .WithName("delete-order")
            .WithSummary("Delete an order (only allowed for Created orders)");

        return app;
    }

    private static async Task<IResult> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        ISender sender = default!)
    {
        var result = await sender.Send(new GetOrdersPagedQuery(page, pageSize));
        return Results.Ok(result);
    }

    private static async Task<IResult> GetOrder(Guid id, ISender sender)
    {
        var order = await sender.Send(new GetOrderQuery(id));
        return order is not null ? Results.Ok(order) : Results.NotFound();
    }

    private static async Task<IResult> GetOrdersByCustomer(
        Guid customerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        ISender sender = default!)
    {
        var result = await sender.Send(new GetOrdersByCustomerQuery(customerId, page, pageSize));
        return Results.Ok(result);
    }

    private static async Task<IResult> GetOrdersByStatus(OrderStatus status, ISender sender)
    {
        var orders = await sender.Send(new GetOrdersByStatusQuery(status));
        return Results.Ok(orders);
    }

    private static async Task<IResult> CreateOrder(CreateOrderDto dto, ISender sender)
    {
        var order = await sender.Send(new CreateOrderCommand(dto));
        return Results.Created($"/api/orders/{order.Id}", order);
    }

    private static async Task<IResult> UpdateOrderStatus(
        Guid id,
        UpdateOrderStatusDto dto,
        ISender sender)
    {
        var order = await sender.Send(new UpdateOrderStatusCommand(id, dto));
        return order is not null ? Results.Ok(order) : Results.NotFound();
    }

    private static async Task<IResult> AddOrderLine(
        Guid id,
        AddOrderLineDto dto,
        ISender sender)
    {
        var order = await sender.Send(new AddOrderLineCommand(id, dto));
        return order is not null ? Results.Ok(order) : Results.NotFound();
    }

    private static async Task<IResult> UpdateOrderLine(
        Guid id,
        UpdateOrderLineDto dto,
        ISender sender)
    {
        var order = await sender.Send(new UpdateOrderLineCommand(id, dto));
        return order is not null ? Results.Ok(order) : Results.NotFound();
    }

    private static async Task<IResult> RemoveOrderLine(
        Guid id,
        Guid orderLineId,
        ISender sender)
    {
        var order = await sender.Send(new RemoveOrderLineCommand(id, orderLineId));
        return order is not null ? Results.Ok(order) : Results.NotFound();
    }

    private static async Task<IResult> DeleteOrder(Guid id, ISender sender)
    {
        var deleted = await sender.Send(new DeleteOrderCommand(id));
        return deleted ? Results.NoContent() : Results.NotFound();
    }
}