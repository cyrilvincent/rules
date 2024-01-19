using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Entities.Sample
{
    public class MyEntity : IBREEntity
    {
        public int MyInt { get; set; }
        public string Text { get; set; }
        public EnumSample MyEnum { get; set; }

    }
}
