using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class DomainRuleException : Exception
    {
        public DomainRuleException(string message) : base(message) { }
    }
}
