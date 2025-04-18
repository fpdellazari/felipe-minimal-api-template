﻿using FelipeMinimalApiTemplate.Models.DTOs;
using FluentValidation;

namespace FelipeMinimalApiTemplate.Validators;

public class PartialUpdateCustomerValidator : AbstractValidator<PartialUpdateCustomerDTO>
{
    public PartialUpdateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .Length(2, 150).WithMessage("O nome deve ter entre 2 e 150 caracteres.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("O e-mail não é válido.");

        RuleFor(x => x.Age)
            .InclusiveBetween(18, 120).WithMessage("A idade deve ser entre 18 e 120 anos.");
    }
}

