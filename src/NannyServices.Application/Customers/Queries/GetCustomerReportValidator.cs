using FluentValidation;

namespace NannyServices.Application.Customers.Queries;

public sealed class GetCustomerReportValidator : AbstractValidator<GetCustomerReportQuery>
{
    public GetCustomerReportValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
    }
}