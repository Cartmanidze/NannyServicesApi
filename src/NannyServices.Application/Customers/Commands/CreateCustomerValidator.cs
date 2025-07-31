using FluentValidation;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Customers.Commands;

public sealed class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Address)
            .SetValidator(new AddressDtoValidator());
    }
}


internal sealed class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.Country).NotEmpty();
        RuleFor(x => x.PostalCode).NotEmpty();
    }
}