using Application.Implementations.Products.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.Products.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(120).WithMessage("Nome até 120 caracteres");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Preço não pode ser negativo");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Estoque não pode ser negativo");
        }
    }
}
