using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class AndOperator : BinaryOperator
    {
        public AndOperator(Operand left, Operand right) : base("and", left, right) { }
    }
}
