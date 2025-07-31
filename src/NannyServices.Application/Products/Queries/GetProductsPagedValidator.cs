using FluentValidation;

namespace NannyServices.Application.Products.Queries;

public sealed class GetProductsPagedValidator : AbstractValidator<GetProductsPagedQuery>
{
    public GetProductsPagedValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(100);
    }
}