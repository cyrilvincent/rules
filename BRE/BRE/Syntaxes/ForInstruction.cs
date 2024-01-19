using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class ForInstruction : KeywordBinaryInstruction
    {
        public ForInstruction(Operand left, Operand right, Bloc bloc) : base("foreach", left, right, bloc) { }

        public override string CSharp()
        {
            var s = $"foreach ({Left.CSharp()} in {Right.CSharp()})\n";
            s += Bloc.CSharp();
            return s.Replace("\n\n", "\n");
        }
    }
}
