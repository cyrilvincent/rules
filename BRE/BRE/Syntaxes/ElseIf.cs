using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class ElseIfInstruction : KeywordInstruction
    {
        public ElseIfInstruction(Operand operand, Bloc bloc) : base("else if", operand, bloc) { }
    }
}
