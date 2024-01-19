using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class KeywordBinaryInstruction : BinaryOperator
    {
        public Bloc Bloc { get; set; }

        public KeywordBinaryInstruction(string name, Operand left, Operand right, Bloc bloc) : base(name, left, right)
        {
            Bloc = bloc;
        }
    }
}
