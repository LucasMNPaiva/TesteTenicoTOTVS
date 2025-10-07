using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public sealed class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = default!;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        private Product() { }

        public Product(string name, decimal price, int stock)
        {
            SetState(name, price, stock);
        }

        // Mantém as mesmas invariantes numa atualização
        public void Update(string name, decimal price, int stock)
        {
            SetState(name, price, stock);
        }

        private void SetState(string name, decimal price, int stock)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required", nameof(name));

            name = name.Trim();
            if (name.Length > 120)
                throw new ArgumentOutOfRangeException(nameof(name), "Name must be ≤ 120 chars");

            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative");

            if (stock < 0)
                throw new ArgumentOutOfRangeException(nameof(stock), "Stock cannot be negative");

            Name = name;
            Price = price;
            Stock = stock;
        }
    }
}
