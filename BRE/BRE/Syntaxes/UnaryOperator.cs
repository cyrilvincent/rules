using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class UnaryOperator : Operator
    {
        public Operand Operand { get; set; }

        public UnaryOperator(string name, Operand operand) : base(name)
        {
            Operand = operand;
        }

        public override string CSharp()
        {
            var o = Operand.CSharp();
            if (o.Contains(" "))
                o = $"({o})";
            return $"{Name} {o}";
        }
    }
}
