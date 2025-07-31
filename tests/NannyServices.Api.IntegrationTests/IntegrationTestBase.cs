
using NannyServices.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace NannyServices.Api.IntegrationTests;

public abstract class IntegrationTestBase(IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
{
    protected readonly HttpClient HttpClient = factory.HttpClient;

    protected async Task<T> ExecuteWithServiceAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = factory.Services.CreateScope();
        return await action(scope.ServiceProvider);
    }

    protected async Task<T?> FindAsync<T, TKey>(TKey id) where T : class
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await dbContext.FindAsync<T>(id);
    }

    protected async Task AddAsync<T>(T entity) where T : class
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => factory.ResetDatabaseAsync();
} 