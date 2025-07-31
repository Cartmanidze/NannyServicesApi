using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Entities;
using NannyServices.Domain.Enums;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Services;

public class OrderService(IUnitOfWork unitOfWork)
{
    public async Task<OrderDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await unitOfWork.Orders.GetByIdWithDetailsAsync(id, cancellationToken);
        return order?.ToDto();
    }

    public async Task<PagedResultDto<OrderDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var orders = await unitOfWork.Orders.GetPagedAsync(page, pageSize, cancellationToken);
        var totalCount = await unitOfWork.Orders.CountAsync(cancellationToken: cancellationToken);

        return orders.ToPagedDto(totalCount, page, pageSize, o => o.ToDto());
    }

    public async Task<PagedResultDto<OrderDto>> GetPagedByCustomerAsync(Guid customerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var orders = await unitOfWork.Orders.GetPagedByCustomerAsync(customerId, page, pageSize, cancellationToken);
        var totalCount = await unitOfWork.Orders.CountAsync(o => o.CustomerId == customerId, cancellationToken);

        return orders.ToPagedDto(totalCount, page, pageSize, o => o.ToDto());
    }

    public async Task<IEnumerable<OrderDto>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
        var orders = await unitOfWork.Orders.GetByStatusAsync(status, cancellationToken);
        return orders.Select(o => o.ToDto());
    }

    public async Task<IEnumerable<OrderDto>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var orders = await unitOfWork.Orders.GetByCustomerIdAsync(customerId, cancellationToken);
        return orders.Select(o => o.ToDto());
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto createOrderDto, CancellationToken cancellationToken = default)
    {
        var customer = await unitOfWork.Customers.GetByIdAsync(createOrderDto.CustomerId, cancellationToken);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {createOrderDto.CustomerId} not found");
        }

        var order = new Order(customer);

        await unitOfWork.Orders.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var createdOrder = await unitOfWork.Orders.GetByIdWithDetailsAsync(order.Id, cancellationToken);
        return createdOrder!.ToDto();
    }

    public async Task<OrderDto?> UpdateStatusAsync(Guid id, UpdateOrderStatusDto updateStatusDto, CancellationToken cancellationToken = default)
    {
        var order = await unitOfWork.Orders.GetByIdWithDetailsAsync(id, cancellationToken);
        if (order == null)
        {
            return null; 
        }
        
        order.ChangeStatus(updateStatusDto.Status);

        await unitOfWork.Orders.UpdateAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return order.ToDto();
    }

    public async Task<OrderDto?> AddOrderLineAsync(Guid orderId, AddOrderLineDto addOrderLineDto, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var order = await unitOfWork.Orders.GetByIdWithDetailsAsync(orderId, cancellationToken);
            if (order == null)
            {
                return null;
            }
            
            var product = await unitOfWork.Products.GetByIdAsync(addOrderLineDto.ProductId, cancellationToken);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {addOrderLineDto.ProductId} not found");
            }

            order.AddOrderLine(product, addOrderLineDto.Count);

            await unitOfWork.Orders.UpdateAsync(order, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return order.ToDto();
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<OrderDto?> UpdateOrderLineAsync(Guid orderId, UpdateOrderLineDto updateOrderLineDto, CancellationToken cancellationToken = default)
    {
        var order = await unitOfWork.Orders.GetByIdWithDetailsAsync(orderId, cancellationToken);
        if (order == null)
        {
            return null;
        }
        
        order.UpdateOrderLineCount(updateOrderLineDto.OrderLineId, updateOrderLineDto.Count);

        await unitOfWork.Orders.UpdateAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return order.ToDto();
    }

    public async Task<OrderDto?> RemoveOrderLineAsync(Guid orderId, Guid orderLineId, CancellationToken cancellationToken = default)
    {
        var order = await unitOfWork.Orders.GetByIdWithDetailsAsync(orderId, cancellationToken);
        if (order == null)
        {
            return null;
        }
        
        order.RemoveOrderLine(orderLineId);

        await unitOfWork.Orders.UpdateAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return order.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await unitOfWork.Orders.GetByIdAsync(id, cancellationToken);
        if (order == null)
        {
            return false;
        }
        
        if (order.Status != OrderStatus.Created)
        {
            throw new InvalidOperationException($"Cannot delete order with status {order.Status}. Only Created orders can be deleted.");
        }

        await unitOfWork.Orders.DeleteAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}