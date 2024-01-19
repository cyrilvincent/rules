using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class NotOperator : UnaryOperator
    {
        public NotOperator(Operand operand) : base("not", operand) { }
    }
}
