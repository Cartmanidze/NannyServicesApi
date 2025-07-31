using System.Net.Http.Json;
using FluentAssertions;
using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;
using NannyServices.Domain.Enums;
using System.Net;
using NannyServices.Api.IntegrationTests.Helpers;

namespace NannyServices.Api.IntegrationTests.Endpoints;

public class OrderEndpointsTests(IntegrationTestWebAppFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateOrder_ShouldAcceptRequest_WhenRequestIsValid()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var dto = TestDataFactory.CreateValidOrderDto(customerId);

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/orders", dto);

        // Assert
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task GetOrders_ShouldReturnPagedResult()
    {
        var customer = TestDataFactory.CreateCustomerEntity();
        await AddAsync(customer);
        for(var i=0;i<8;i++)
            await HttpClient.PostAsJsonAsync("/api/orders", new CreateOrderDto(customer.Id));
        var response = await HttpClient.GetAsync("/api/orders?page=2&pageSize=5");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var paged = await response.Content.ReadFromJsonAsync<PagedResultDto<OrderDto>>();
        paged!.Items.Should().HaveCount(3);
        paged.TotalCount.Should().Be(8);
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/api/orders/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetOrdersByCustomer_ShouldReturnResults()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/api/orders/customer/{customerId}");

        // Assert
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task GetOrdersByStatus_ShouldReturnResults()
    {
        // Arrange
        var status = OrderStatus.Completed;

        // Act
        var response = await HttpClient.GetAsync($"/api/orders/status/{status}");

        // Assert
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateOrderStatus_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var dto = new UpdateOrderStatusDto(OrderStatus.Completed);

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/api/orders/{nonExistentId}/status", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await HttpClient.DeleteAsync($"/api/orders/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddOrderLine_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        var nonExistentOrderId = Guid.NewGuid();
        var nonExistentProductId = Guid.NewGuid();
        var dto = TestDataFactory.CreateValidAddOrderLineDto(nonExistentProductId);

        // Act
        var response = await HttpClient.PostAsJsonAsync($"/api/orders/{nonExistentOrderId}/order-lines", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateOrderLine_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        var nonExistentOrderId = Guid.NewGuid();
        var dto = TestDataFactory.CreateValidUpdateOrderLineDto();

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/api/orders/{nonExistentOrderId}/order-lines", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemoveOrderLine_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        var nonExistentOrderId = Guid.NewGuid();
        var nonExistentProductId = Guid.NewGuid();

        // Act
        var response = await HttpClient.DeleteAsync($"/api/orders/{nonExistentOrderId}/order-lines/{nonExistentProductId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnBadRequest_WhenCustomerIdIsEmpty()
    {
        // Arrange
        var dto = TestDataFactory.CreateInvalidOrderDto();

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/orders", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddOrderLine_ShouldReturnBadRequest_WhenQuantityIsZero()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomerEntity();
        var product = TestDataFactory.CreateProductEntity();
        await AddAsync(customer);
        await AddAsync(product);
        var orderResponse = await HttpClient.PostAsJsonAsync("/api/orders", new CreateOrderDto(customer.Id));
        var orderDto = await orderResponse.Content.ReadFromJsonAsync<OrderDto>();
        var dto = TestDataFactory.CreateInvalidAddOrderLineDto();

        // Act
        var response = await HttpClient.PostAsJsonAsync($"/api/orders/{orderDto!.Id}/order-lines", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
} 