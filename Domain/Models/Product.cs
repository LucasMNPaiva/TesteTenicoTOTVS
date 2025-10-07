using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public sealed class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        private Product() { }
         
        public Product(string name, decimal price, int stock) => Update(name, price, stock);

        public void Update(string name, decimal price, int stock)
        {
            Name = name.Trim();
            Price = price;
            Stock = stock;
        }
    }
}
