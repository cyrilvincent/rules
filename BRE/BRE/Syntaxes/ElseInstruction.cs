using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class ElseInstruction : KeywordInstruction
    {
        public ElseInstruction(Operand operand, Bloc bloc) : base("else", new Operand(true), bloc) { }
    }
}
