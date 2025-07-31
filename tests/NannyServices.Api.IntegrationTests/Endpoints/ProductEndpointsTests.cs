using System.Net.Http.Json;
using FluentAssertions;
using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;
using NannyServices.Domain.Entities;
using System.Net;
using NannyServices.Api.IntegrationTests.Helpers;

namespace NannyServices.Api.IntegrationTests.Endpoints;

public class ProductEndpointsTests(IntegrationTestWebAppFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateProduct_ShouldAcceptRequest_WhenRequestIsValid()
    {
        var dto = TestDataFactory.CreateValidProductDto();
        var response = await HttpClient.PostAsJsonAsync("/api/products", dto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<ProductDto>();
        var entity = await FindAsync<Product, Guid>(created!.Id);
        entity.Should().NotBeNull();
        entity!.Name.Should().Be(dto.Name);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnPagedResult()
    {
        for(var i=0;i<12;i++)
            await AddAsync(TestDataFactory.CreateProductEntity($"Prod{i}", 10+i));
        var response = await HttpClient.GetAsync("/api/products?page=2&pageSize=5");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var paged = await response.Content.ReadFromJsonAsync<PagedResultDto<ProductDto>>();
        paged!.Items.Should().HaveCount(5);
        paged.TotalCount.Should().Be(12);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnAllProducts()
    {
        await AddAsync(TestDataFactory.CreateProductEntity("Milk", 2));
        await AddAsync(TestDataFactory.CreateProductEntity("Bread", 1));
        var response = await HttpClient.GetAsync("/api/products/all");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
        list!.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/api/products/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SearchProducts_ShouldReturnResults_WhenNameProvided()
    {
        await AddAsync(TestDataFactory.CreateProductEntity("Milk", 3));
        await AddAsync(TestDataFactory.CreateProductEntity("Bread", 1));
        var response = await HttpClient.GetAsync("/api/products/search?name=mil");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
        list!.Should().ContainSingle(p => p.Name == "Milk");
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateDto = TestDataFactory.CreateValidUpdateProductDto();

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/api/products/{nonExistentId}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await HttpClient.DeleteAsync($"/api/products/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var dto = TestDataFactory.CreateInvalidProductDto();

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/products", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnBadRequest_WhenPriceIsNegative()
    {
        // Arrange
        var dto = TestDataFactory.CreateValidProductDto(amount: -10m);

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/products", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
} 