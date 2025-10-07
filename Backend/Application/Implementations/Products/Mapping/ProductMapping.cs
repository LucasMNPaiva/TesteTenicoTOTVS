using Application.Implementations.Products.Dtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.Products.Mapping
{
    public static class ProductMapping
    {
        public static ProductDto ToDto(this Product p)
            => new(p.Id, p.Name, p.Price, p.Stock);
    }
}
