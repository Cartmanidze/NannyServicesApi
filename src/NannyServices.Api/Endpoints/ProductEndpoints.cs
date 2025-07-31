using MediatR;
using Microsoft.AspNetCore.Mvc;
using NannyServices.Application.Products.Commands;
using NannyServices.Application.Products.Queries;
using NannyServices.Application.DTOs;

namespace NannyServices.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products")
            .WithOpenApi();

        // Queries
        group.MapGet("/", GetProducts)
            .WithName("get-products")
            .WithSummary("Get all products with pagination");

        group.MapGet("/all", GetAllProducts)
            .WithName("get-all-products")
            .WithSummary("Get all products without pagination");

        group.MapGet("/{id:guid}", GetProduct)
            .WithName("get-product-by-id")
            .WithSummary("Get product by ID");

        group.MapGet("/search", SearchProducts)
            .WithName("search-products")
            .WithSummary("Search products by name");

        // Commands
        group.MapPost("/", CreateProduct)
            .WithName("create-product")
            .WithSummary("Create a new product");

        group.MapPut("/{id:guid}", UpdateProduct)
            .WithName("update-product")
            .WithSummary("Update an existing product");

        group.MapDelete("/{id:guid}", DeleteProduct)
            .WithName("delete-product")
            .WithSummary("Delete a product");

        return app;
    }

    private static async Task<IResult> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        ISender sender = null!)
    {
        var result = await sender.Send(new GetProductsPagedQuery(page, pageSize));
        return Results.Ok(result);
    }

    private static async Task<IResult> GetAllProducts(ISender sender)
    {
        var products = await sender.Send(new GetAllProductsQuery());
        return Results.Ok(products);
    }

    private static async Task<IResult> GetProduct(Guid id, ISender sender)
    {
        var product = await sender.Send(new GetProductQuery(id));
        return product is not null ? Results.Ok(product) : Results.NotFound();
    }

    private static async Task<IResult> SearchProducts(
        [FromQuery] string? searchTerm,
        ISender sender)
    {
        var products = await sender.Send(new SearchProductsQuery(searchTerm ?? string.Empty));
        return Results.Ok(products);
    }

    private static async Task<IResult> CreateProduct(CreateProductDto dto, ISender sender)
    {
        var product = await sender.Send(new CreateProductCommand(dto));
        return Results.Created($"/api/products/{product.Id}", product);
    }

    private static async Task<IResult> UpdateProduct(Guid id, UpdateProductDto dto, ISender sender)
    {
        var product = await sender.Send(new UpdateProductCommand(id, dto));
        return product is not null ? Results.Ok(product) : Results.NotFound();
    }

    private static async Task<IResult> DeleteProduct(Guid id, ISender sender)
    {
        var deleted = await sender.Send(new DeleteProductCommand(id));
        return deleted ? Results.NoContent() : Results.NotFound();
    }
}