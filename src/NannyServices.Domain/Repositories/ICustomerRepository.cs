using NannyServices.Domain.Entities;

namespace NannyServices.Domain.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByIdWithOrdersAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
}