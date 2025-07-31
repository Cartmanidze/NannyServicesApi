using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Products.Commands;

public sealed record CreateProductCommand(CreateProductDto Dto) : IRequest<ProductDto>;