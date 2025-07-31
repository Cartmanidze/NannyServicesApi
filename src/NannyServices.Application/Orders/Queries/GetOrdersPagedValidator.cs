using FluentValidation;

namespace NannyServices.Application.Orders.Queries;

public sealed class GetOrdersPagedValidator : AbstractValidator<GetOrdersPagedQuery>
{
    public GetOrdersPagedValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(100);
    }
}