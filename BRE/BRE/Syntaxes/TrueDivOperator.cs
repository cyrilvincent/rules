using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class TrueDivOperator : BinaryOperator
    {
        public TrueDivOperator(Operand left, Operand right) : base("-", left, right) { }

        public override string CSharp()
        {
            return $"(int){Left.CSharp()} / (int){Right.CSharp()}";
        }
    }
}
