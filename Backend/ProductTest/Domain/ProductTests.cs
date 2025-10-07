using Domain.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductTest.Domain
{
    public class ProductTests
    {
        [Fact]
        public void ctor_deve_criar_produto_valido()
        {
            var p = new Product("Ração Premium", 99.9m, 10);

            p.Id.Should().NotBeEmpty();
            p.Name.Should().Be("Ração Premium");
            p.Price.Should().Be(99.9m);
            p.Stock.Should().Be(10);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ctor_deve_falhar_quando_nome_invalido(string? nome)
        {
            Action act = () => new Product(nome!, 10m, 1);
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(-1)]
        public void ctor_deve_falhar_quando_preco_negativo(decimal preco)
        {
            Action act = () => new Product("Item", preco, 1);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void update_deve_alterar_campos_respeitando_invariantes()
        {
            var p = new Product("Item", 10m, 1);
            p.Update("Novo", 20m, 5);

            p.Name.Should().Be("Novo");
            p.Price.Should().Be(20m);
            p.Stock.Should().Be(5);
        }
    }
}
