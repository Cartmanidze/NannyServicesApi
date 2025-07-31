using FluentValidation;

namespace NannyServices.Application.Orders.Queries;

public sealed class GetOrdersByCustomerValidator : AbstractValidator<GetOrdersByCustomerQuery>
{
    public GetOrdersByCustomerValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(100);
    }
}