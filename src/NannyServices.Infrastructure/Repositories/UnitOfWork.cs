using Microsoft.EntityFrameworkCore.Storage;
using NannyServices.Domain.Repositories;
using NannyServices.Infrastructure.Data;

namespace NannyServices.Infrastructure.Repositories;

public class UnitOfWork(
    ApplicationDbContext context,
    ICustomerRepository customers,
    IProductRepository products,
    IOrderRepository orders)
    : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public ICustomerRepository Customers { get; } = customers;
    public IProductRepository Products { get; } = products;
    public IOrderRepository Orders { get; } = orders;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        context.Dispose();
    }
}