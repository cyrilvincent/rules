using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class AffectationOperator : BinaryOperator
    {
        public AffectationOperator(Operand left, Operand right) : base("=", left, right) { }

        public override string CSharp()
        {
            return $"{Left.CSharp()} = {Right.CSharp()}";
        }
    }
}
