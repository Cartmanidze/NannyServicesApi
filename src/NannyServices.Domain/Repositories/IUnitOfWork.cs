namespace NannyServices.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }
    
    IProductRepository Products { get; }
    
    IOrderRepository Orders { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    
    void MarkAsAdded<TEntity>(TEntity entity) where TEntity : class;
}