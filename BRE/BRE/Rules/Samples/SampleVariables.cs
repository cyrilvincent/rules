using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Rules.Sample
{
    public class SampleVariables : Variables
    {
        public SampleVariables()
        {
            Variable v = new Variable{Name="Variable1",Value = 1};
            Add(v);
            v = new Variable { Name = "Variable2", Value = 2 };
            Add(v);
            v = new Variable { Name = "Variable3", Value = "VariableText" };
            Add(v);
            List<int> l = new List<int> {1,2,3,4,5};
            v = new Variable { Name = "ValueList1", Value = l};
            Add(v);

        }
    }
}
