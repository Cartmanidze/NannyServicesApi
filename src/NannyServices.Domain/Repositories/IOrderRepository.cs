using NannyServices.Domain.Entities;
using NannyServices.Domain.Enums;

namespace NannyServices.Domain.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Order>> GetByCustomerIdAndDateRangeAsync(
        Guid customerId, 
        DateTime startDate, 
        DateTime endDate, 
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Order>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Order>> GetPagedByCustomerAsync(Guid customerId, int page, int pageSize, CancellationToken cancellationToken = default);
}