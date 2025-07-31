using MediatR;

namespace NannyServices.Application.Products.Commands;

public sealed record DeleteProductCommand(Guid Id) : IRequest<bool>;