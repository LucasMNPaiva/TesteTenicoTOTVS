using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.Products.Commands
{
    public record CreateProductCommand(string Name, decimal Price, int Stock);
}
