using Microsoft.EntityFrameworkCore;
using NannyServices.Domain.Entities;
using NannyServices.Domain.Repositories;
using NannyServices.Infrastructure.Data;

namespace NannyServices.Infrastructure.Repositories;

public class CustomerRepository(ApplicationDbContext context) : Repository<Customer>(context), ICustomerRepository
{
    public async Task<Customer?> GetByIdWithOrdersAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Orders)
                .ThenInclude(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .OrderBy(c => c.Name)
            .ThenBy(c => c.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Customer>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAllAsync(cancellationToken);
        }

        var lowerSearchTerm = searchTerm.ToLower();
        return await DbSet
            .Where(c => c.Name.ToLower().Contains(lowerSearchTerm) ||
                       c.LastName.ToLower().Contains(lowerSearchTerm))
            .OrderBy(c => c.Name)
            .ThenBy(c => c.LastName)
            .ToListAsync(cancellationToken);
    }
}