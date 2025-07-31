using System.Net.Http.Json;
using FluentAssertions;
using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;
using NannyServices.Domain.Entities;
using System.Net;
using NannyServices.Api.IntegrationTests.Helpers;

namespace NannyServices.Api.IntegrationTests.Endpoints;

public class CustomerEndpointsTests(IntegrationTestWebAppFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateCustomer_ShouldCreateCustomer_WhenRequestIsValid()
    {
        // Arrange
        var dto = TestDataFactory.CreateValidCustomerDto();

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/customers", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<CustomerDto>();
        created.Should().NotBeNull();
        created.Name.Should().Be(dto.Name);
        created.LastName.Should().Be(dto.LastName);

        var entity = await FindAsync<Customer, Guid>(created.Id);
        entity.Should().NotBeNull();
        entity!.Name.Should().Be(dto.Name);
    }

    [Fact]
    public async Task GetCustomers_ShouldReturnPagedResult()
    {
        // Arrange – seed 12 customers
        for (int i = 0; i < 12; i++)
        {
            await AddAsync(TestDataFactory.CreateCustomerEntity($"Name{i}", $"Last{i}"));
        }

        // Act
        var response = await HttpClient.GetAsync("/api/customers?page=2&pageSize=5");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var paged = await response.Content.ReadFromJsonAsync<PagedResultDto<CustomerDto>>();

        // Assert
        paged.Should().NotBeNull();
        paged!.Items.Should().HaveCount(5);
        paged.TotalCount.Should().Be(12);
        paged.Page.Should().Be(2);
    }

    [Fact]
    public async Task GetCustomerById_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/api/customers/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SearchCustomers_ShouldReturnResults_WhenNameProvided()
    {
        // Arrange
        await AddAsync(TestDataFactory.CreateCustomerEntity("John", "Smith"));
        await AddAsync(TestDataFactory.CreateCustomerEntity("Alice", "Cooper"));

        // Act
        var response = await HttpClient.GetAsync("/api/customers/search?searchTerm=joH");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content.ReadFromJsonAsync<List<CustomerDto>>();

        // Assert
        list.Should().HaveCount(1);
        list!.First().Name.Should().Be("John");
    }

    [Fact]
    public async Task UpdateCustomer_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateDto = TestDataFactory.CreateValidUpdateCustomerDto();

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/api/customers/{nonExistentId}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteCustomer_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await HttpClient.DeleteAsync($"/api/customers/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCustomerWithOrders_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/api/customers/{nonExistentId}/with-orders");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCustomerReport_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var startDate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
        var endDate = DateTime.Now.ToString("yyyy-MM-dd");

        // Act
        var response = await HttpClient.GetAsync($"/api/customers/{nonExistentId}/reports?startDate={startDate}&endDate={endDate}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var dto = TestDataFactory.CreateInvalidCustomerDto();

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/customers", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
} 