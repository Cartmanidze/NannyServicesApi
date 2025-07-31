using MediatR;
using Microsoft.AspNetCore.Mvc;
using NannyServices.Application.Customers.Commands;
using NannyServices.Application.Customers.Queries;
using NannyServices.Application.DTOs;

namespace NannyServices.Api.Endpoints;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/customers")
            .WithTags("Customers")
            .WithOpenApi();

        // Queries
        group.MapGet("/", GetCustomers)
            .WithName("get-customers")
            .WithSummary("Get all customers with pagination");

        group.MapGet("/{id:guid}", GetCustomer)
            .WithName("get-customer-by-id")
            .WithSummary("Get customer by ID");

        group.MapGet("/{id:guid}/with-orders", GetCustomerWithOrders)
            .WithName("get-customer-with-orders")
            .WithSummary("Get customer by ID with their orders");

        group.MapGet("/search", SearchCustomers)
            .WithName("search-customers")
            .WithSummary("Search customers by name");

        group.MapGet("/{id:guid}/reports", GetCustomerReport)
            .WithName("get-customer-report")
            .WithSummary("Get customer report for a date range");

        group.MapGet("/{id:guid}/reports/week", GetCustomerWeeklyReport)
            .WithName("get-customer-weekly-report")
            .WithSummary("Get customer report for the current week");

        group.MapGet("/{id:guid}/reports/month", GetCustomerMonthlyReport)
            .WithName("get-customer-monthly-report")
            .WithSummary("Get customer report for the current month");

        // Commands
        group.MapPost("/", CreateCustomer)
            .WithName("create-customer")
            .WithSummary("Create a new customer");

        group.MapPut("/{id:guid}", UpdateCustomer)
            .WithName("update-customer")
            .WithSummary("Update an existing customer");

        group.MapDelete("/{id:guid}", DeleteCustomer)
            .WithName("delete-customer")
            .WithSummary("Delete a customer");

        return app;
    }

    private static async Task<IResult> GetCustomers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        ISender sender = default!)
    {
        var result = await sender.Send(new GetCustomersPagedQuery(page, pageSize));
        return Results.Ok(result);
    }

    private static async Task<IResult> GetCustomer(Guid id, ISender sender)
    {
        var customer = await sender.Send(new GetCustomerQuery(id));
        return customer is not null ? Results.Ok(customer) : Results.NotFound();
    }

    private static async Task<IResult> GetCustomerWithOrders(Guid id, ISender sender)
    {
        var customer = await sender.Send(new GetCustomerWithOrdersQuery(id));
        return customer is not null ? Results.Ok(customer) : Results.NotFound();
    }

    private static async Task<IResult> SearchCustomers(
        [FromQuery] string? searchTerm,
        ISender sender)
    {
        var customers = await sender.Send(new SearchCustomersQuery(searchTerm ?? string.Empty));
        return Results.Ok(customers);
    }

    private static async Task<IResult> GetCustomerReport(
        Guid id,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        ISender sender)
    {
        var report = await sender.Send(new GetCustomerReportQuery(id, startDate, endDate));
        return report is not null ? Results.Ok(report) : Results.NotFound();
    }

    private static async Task<IResult> GetCustomerWeeklyReport(Guid id, ISender sender)
    {
        var today = DateTime.Today;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(7);

        var report = await sender.Send(new GetCustomerReportQuery(id, startOfWeek, endOfWeek));
        return report is not null ? Results.Ok(report) : Results.NotFound();
    }

    private static async Task<IResult> GetCustomerMonthlyReport(Guid id, ISender sender)
    {
        var today = DateTime.Today;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1);

        var report = await sender.Send(new GetCustomerReportQuery(id, startOfMonth, endOfMonth));
        return report is not null ? Results.Ok(report) : Results.NotFound();
    }

    private static async Task<IResult> CreateCustomer(CreateCustomerDto dto, ISender sender)
    {
        var customer = await sender.Send(new CreateCustomerCommand(dto));
        return Results.Created($"/api/customers/{customer.Id}", customer);
    }

    private static async Task<IResult> UpdateCustomer(Guid id, UpdateCustomerDto dto, ISender sender)
    {
        var customer = await sender.Send(new UpdateCustomerCommand(id, dto));
        return customer is not null ? Results.Ok(customer) : Results.NotFound();
    }

    private static async Task<IResult> DeleteCustomer(Guid id, ISender sender)
    {
        var deleted = await sender.Send(new DeleteCustomerCommand(id));
        return deleted ? Results.NoContent() : Results.NotFound();
    }
}