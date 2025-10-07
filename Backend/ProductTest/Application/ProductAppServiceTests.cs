using Application.Implementations.Products.Services;
using Domain.Interfaces;
using Domain.Models;
using Application.Implementations.Products.Commands;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ProductTest.Application
{
    public class ProductAppServiceTests
    {
        private readonly IProductRepository _repo = Substitute.For<IProductRepository>();
        private readonly ProductAppService _svc;

        public ProductAppServiceTests() => _svc = new ProductAppService(_repo);

        [Fact]
        public async Task create_deve_persistir_e_retornar_dto()
        {
            var cmd = new CreateProductCommand("Item", 50m, 3);

            // Act
            var dto = await _svc.CreateAsync(cmd);

            // Assert
            dto.Name.Should().Be("Item");
            dto.Price.Should().Be(50m);
            dto.Stock.Should().Be(3);

            await _repo.Received(1).AddAsync(Arg.Any<Product>());
            await _repo.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task update_deve_retornar_false_quando_nao_existe()
        {
            _repo.GetByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult<Product?>(null));

            var ok = await _svc.UpdateAsync(Guid.NewGuid(), new UpdateProductCommand("X", 1, 1));

            ok.Should().BeFalse();
            await _repo.DidNotReceive().UpdateAsync(Arg.Any<Product>());
        }

        [Fact]
        public async Task update_deve_alterar_quando_existe()
        {
            var existing = new Product("Old", 10m, 1);
            _repo.GetByIdAsync(Arg.Any<Guid>()).Returns(existing);

            var ok = await _svc.UpdateAsync(Guid.NewGuid(), new UpdateProductCommand("New", 20m, 5));

            ok.Should().BeTrue();
            existing.Name.Should().Be("New");
            existing.Price.Should().Be(20m);
            existing.Stock.Should().Be(5);

            await _repo.Received(1).UpdateAsync(existing);
            await _repo.Received(1).SaveChangesAsync();
        }
    }
}
