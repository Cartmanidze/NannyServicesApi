using FluentValidation;

namespace NannyServices.Application.Customers.Queries;

public sealed class GetCustomersPagedValidator : AbstractValidator<GetCustomersPagedQuery>
{
    public GetCustomersPagedValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
    }
}