using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class MulOperator : BinaryOperator
    {
        public MulOperator(Operand left, Operand right) : base("*", left, right) { }
    }
}
