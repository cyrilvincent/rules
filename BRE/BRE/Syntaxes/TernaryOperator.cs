using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class TernaryOperator : Operator
    {
        public Operand Operand1 { get; set; }
        public Operand Operand2 { get; set; }
        public Operand Operand3 { get; set; }

        public TernaryOperator(string name, Operand operand1, Operand operand2, Operand operand3) : base(name)
        {
            Operand1 = operand1;
            Operand2 = operand2;
            Operand3 = operand3;
        }
    }
}
