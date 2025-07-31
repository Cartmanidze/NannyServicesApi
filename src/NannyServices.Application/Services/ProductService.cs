using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Services;

public class ProductService(IUnitOfWork unitOfWork)
{
    public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, cancellationToken);
        return product?.ToDto();
    }

    public async Task<PagedResultDto<ProductDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var products = await unitOfWork.Products.GetPagedAsync(page, pageSize, cancellationToken);
        var totalCount = await unitOfWork.Products.CountAsync(cancellationToken: cancellationToken);

        return products.ToPagedDto(totalCount, page, pageSize, p => p.ToDto());
    }

    public async Task<IEnumerable<ProductDto>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var products = await unitOfWork.Products.SearchByNameAsync(searchTerm, cancellationToken);
        return products.Select(p => p.ToDto());
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto, CancellationToken cancellationToken = default)
    {
        var existsWithSameName = await unitOfWork.Products.ExistsByNameAsync(createProductDto.Name, cancellationToken: cancellationToken);
        if (existsWithSameName)
        {
            throw new InvalidOperationException($"Product with name '{createProductDto.Name}' already exists");
        }

        var product = createProductDto.ToEntity();

        await unitOfWork.Products.AddAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.ToDto();
    }

    public async Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto updateProductDto, CancellationToken cancellationToken = default)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            return null;
        }
        
        var existsWithSameName = await unitOfWork.Products.ExistsByNameAsync(updateProductDto.Name, id, cancellationToken);
        if (existsWithSameName)
        {
            throw new InvalidOperationException($"Another product with name '{updateProductDto.Name}' already exists");
        }

        product.UpdateName(updateProductDto.Name);
        product.UpdatePrice(updateProductDto.Price.ToEntity());

        await unitOfWork.Products.UpdateAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            return false;
        }
        
        await unitOfWork.Products.DeleteAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await unitOfWork.Products.GetAllAsync(cancellationToken);
        return products.Select(p => p.ToDto());
    }
}