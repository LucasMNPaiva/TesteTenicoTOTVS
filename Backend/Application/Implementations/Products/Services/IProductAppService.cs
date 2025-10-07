using Application.Implementations.Products.Commands;
using Application.Implementations.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.Products.Services
{
    public interface IProductAppService
    {
        Task<ProductDto> CreateAsync(CreateProductCommand cmd, CancellationToken ct = default);
        Task<List<ProductDto>> ListAsync(CancellationToken ct = default);
        Task<ProductDto?> GetAsync(Guid id, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateProductCommand cmd, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
