using Application.Implementations.Products.Commands;
using Application.Implementations.Products.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductTest.Application
{
    public class CreateProductCommandValidatorTests
    {
        private readonly CreateProductCommandValidator _validator = new();

        [Fact]
        public void deve_validar_ok()
        {
            var cmd = new CreateProductCommand("Item", 10m, 2);
            var r = _validator.TestValidate(cmd);
            r.IsValid.Should().BeTrue();
        }

        [Fact]
        public void deve_invalidar_campos_ruins()
        {
            var cmd = new CreateProductCommand("", -1m, -5);
            var r = _validator.TestValidate(cmd);

            r.ShouldHaveValidationErrorFor(x => x.Name);
            r.ShouldHaveValidationErrorFor(x => x.Price);
            r.ShouldHaveValidationErrorFor(x => x.Stock);
        }
    }
}
