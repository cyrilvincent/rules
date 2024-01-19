﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public class IfInstruction : KeywordInstruction
    {
        public IfInstruction(Operand operand, Bloc bloc) : base("if", operand, bloc) { }

    }
}
