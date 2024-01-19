using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class BinaryOperator : Operator
    {
        public Operand Left { get; set; }
        public Operand Right { get; set; }

        public BinaryOperator(string name, Operand left, Operand right) : base(name) 
        {
            Left = left;
            Right = right;
        }

        public override string CSharp()
        {
            var left = Left.CSharp();
            var right = Right.CSharp();
            if (left.Contains(" "))
                left = $"({left})";
            if (right.Contains(" "))
                right = $"({right})";
            return $"{left} {Name} {right}";
        }
    }
}
