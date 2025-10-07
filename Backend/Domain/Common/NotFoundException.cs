using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string? message = null) : base(message ?? "Not found") { }
    }
}
