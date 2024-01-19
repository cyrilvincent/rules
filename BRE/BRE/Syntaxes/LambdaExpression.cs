using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class LambdaExpression : KeywordInstruction
    {
        public List<string> Parameters { get; set; } = new List<string>();

        public LambdaExpression(List<string> parameters, Bloc bloc) : base("=>", new Operand(null), bloc)
        {
            Parameters = parameters;
        }

        public override string CSharp()
        {
            var s = "(";
            s += String.Join(", ", Parameters);
            s += ") => ";
            s += Bloc.CSharp();
            return s;

        }
    }
}
