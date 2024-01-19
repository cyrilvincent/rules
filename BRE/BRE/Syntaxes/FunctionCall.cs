using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class FunctionCall : UnaryOperator
    {
        public List<Operand> Values { get; set; } = new List<Operand>();

        public FunctionCall(string name, List<Operand> values) : base("call", new Operand(name))
        {
            Values = values;
        }

        public override string CSharp()
        {
            string s = $"{Operand.Value}(";
            s += String.Join(", ", Values.Select(v => v.Value));
            s += ")";
            return s;
        }
    }
}
