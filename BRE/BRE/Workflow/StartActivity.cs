using BRE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Workflow
{
    public class StartActivity : AbstractActivity<BREEntity, BREEntity>
    {
        public override bool Compile()
        {
            Console.WriteLine(this.ToString()+"=False");
            return false;
        }
    }
}
