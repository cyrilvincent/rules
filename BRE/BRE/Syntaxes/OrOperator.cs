using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class OrOperator : BinaryOperator
    {
        public OrOperator(Operand left, Operand right) : base("or", left, right) { }
    }
}
