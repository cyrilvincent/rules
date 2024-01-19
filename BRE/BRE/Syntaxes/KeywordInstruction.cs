using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class KeywordInstruction : UnaryOperator
    {
        public Bloc Bloc { get; set; }

        public KeywordInstruction(string name, Operand operand, Bloc bloc) : base(name, operand)
        {
            Bloc = bloc;
        }


        public override string CSharp()
        {
            var s = $"{Name} ({Operand.CSharp()})";
            s += Bloc.CSharp();
            return s.Replace("\n\n", "\n").Replace("\n;", "\n");
        }
    }
}
