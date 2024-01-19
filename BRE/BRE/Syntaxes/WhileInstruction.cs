using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class WhileInstruction : KeywordInstruction
    {
        public WhileInstruction(Operand operand, Bloc bloc) : base("while", operand, bloc) { }
    }
}
