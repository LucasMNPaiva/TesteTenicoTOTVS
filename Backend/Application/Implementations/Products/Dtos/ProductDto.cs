﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.Products.Dtos
{
    public record ProductDto(Guid Id, string Name, decimal Price, int Stock);
}
