using FelipeMinimalApiTemplate.Models.DTOs;
using FluentValidation;

namespace FelipeMinimalApiTemplate.Validators;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDTO>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(2, 150).WithMessage("O nome deve ter entre 2 e 150 caracteres");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("O e-mail não é válido.");

        RuleFor(x => x.Age)
            .InclusiveBetween(18, 120).WithMessage("A idade deve ser entre 18 e 120 anos.");
    }
}

