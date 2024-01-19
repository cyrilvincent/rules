using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class GEOperator : BinaryOperator
    {
        public GEOperator(Operand left, Operand right) : base(">=", left, right) { }
    }
}
