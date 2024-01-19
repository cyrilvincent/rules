using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Syntaxes
{
    public interface IOperand
    {
        object? Value { get; set; }

        string CSharp();
    }

    public class Operand : IOperand
    {
        public object? Value { get; set; }

        public Operand(object? value)
        {
            Value = value;
        }


        public virtual string CSharp()
        {
            return Value?.ToString() ?? "null";
        }
    }
}
