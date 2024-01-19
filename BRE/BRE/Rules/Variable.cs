using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Rules
{
    public class Variable : IRuleEntity
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Description { get; set; }
    }
}
