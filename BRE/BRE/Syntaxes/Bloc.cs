using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public interface IBloc
    {
        List<Operand> Operands { get; set; }
    }

    public class Bloc : Operand
    {
        public List<Operand> Operands { get; set; } = new List<Operand>();

        public Bloc(List<Operand> operands) : base(null)
        {
            Operands = operands;
        }
        public Bloc() : base(null) { }

        public override string CSharp()
        {
            string s = " {\n";
            foreach (Operand o in Operands)
            {
                s += $"\t{o.CSharp().Replace("\n\t", "\n\t\t")};\n";
            }
            s += "\t}\n";
            return s.Replace("\n;\n", "\n");
        }


    }
}
