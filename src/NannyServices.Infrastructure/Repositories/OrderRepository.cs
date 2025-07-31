using Microsoft.EntityFrameworkCore;
using NannyServices.Domain.Entities;
using NannyServices.Domain.Enums;
using NannyServices.Domain.Repositories;
using NannyServices.Infrastructure.Data;

namespace NannyServices.Infrastructure.Repositories;

public class OrderRepository(ApplicationDbContext context) : Repository<Order>(context), IOrderRepository
{
    public async Task<Order?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(o => o.Customer)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByCustomerIdAndDateRangeAsync(
        Guid customerId, 
        DateTime startDate, 
        DateTime endDate, 
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
            .Where(o => o.CustomerId == customerId &&
                       o.CreatedAt >= startDate &&
                       o.CreatedAt <= endDate)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(o => o.Customer)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(o => o.Customer)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetPagedByCustomerAsync(Guid customerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}