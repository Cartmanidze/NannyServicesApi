using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Products.Commands;

public sealed record UpdateProductCommand(Guid Id, UpdateProductDto Dto) : IRequest<ProductDto?>;