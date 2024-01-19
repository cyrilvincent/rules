using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public interface IOperator
    {
        string Name { get; set; }
    }

    public class Operator : Operand, IOperator
    {
        public string Name { get; set; }

        public Operator(string name) : base(null)
        {
            Name = name;
        }
    }
}
