using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class LTOperator : BinaryOperator
    {
        public LTOperator(Operand left, Operand right) : base("<", left, right) { }
    }
}
