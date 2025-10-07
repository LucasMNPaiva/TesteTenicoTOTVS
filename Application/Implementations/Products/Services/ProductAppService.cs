using Application.Implementations.Products.Commands;
using Application.Implementations.Products.Dtos;
using Application.Implementations.Products.Mapping;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.Products.Services
{
    public class ProductAppService(IProductRepository repo) : IProductAppService
    {
        public async Task<ProductDto> CreateAsync(CreateProductCommand cmd, CancellationToken ct = default)
        {
            var entity = new Product(cmd.Name, cmd.Price, cmd.Stock);
            await repo.AddAsync(entity, ct);
            await repo.SaveChangesAsync(ct);
            return entity.ToDto();
        }

        public async Task<List<ProductDto>> ListAsync(CancellationToken ct = default)
            => (await repo.ListAsync(ct)).Select(p => p.ToDto()).ToList();

        public async Task<ProductDto?> GetAsync(int id, CancellationToken ct = default)
            => (await repo.GetByIdAsync(id, ct))?.ToDto();

        public async Task<bool> UpdateAsync(int id, UpdateProductCommand cmd, CancellationToken ct = default)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return false;
            entity.Update(cmd.Name, cmd.Price, cmd.Stock);
            await repo.UpdateAsync(entity, ct);
            await repo.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return false;
            await repo.DeleteAsync(entity, ct);
            await repo.SaveChangesAsync(ct);
            return true;
        }
    }
}
