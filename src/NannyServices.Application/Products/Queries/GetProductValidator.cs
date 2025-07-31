using FluentValidation;

namespace NannyServices.Application.Products.Queries;

public sealed class GetProductValidator : AbstractValidator<GetProductQuery>
{
    public GetProductValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}