using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository(AppDbContext db) : IProductRepository
    {
        public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
            => db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

        public Task<List<Product>> ListAsync(CancellationToken ct = default)
            => db.Products.OrderBy(p => p.Name).ToListAsync(ct);

        public Task AddAsync(Product product, CancellationToken ct = default)
        { db.Products.Add(product); return Task.CompletedTask; }

        public Task UpdateAsync(Product product, CancellationToken ct = default)
        { db.Products.Update(product); return Task.CompletedTask; }

        public Task DeleteAsync(Product product, CancellationToken ct = default)
        { db.Products.Remove(product); return Task.CompletedTask; }

        public Task SaveChangesAsync(CancellationToken ct = default)
            => db.SaveChangesAsync(ct);
    }
}
