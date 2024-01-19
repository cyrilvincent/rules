using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class ReturnOperator : UnaryOperator
    {
        public ReturnOperator(Operand operand) : base("return", operand) { }

        public override string CSharp()
        {
            return $"return {Operand.CSharp()}";
        }
    }
}
