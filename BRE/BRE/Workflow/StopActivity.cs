using BRE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Workflow
{
    public class StopActivity : AbstractActivity<BREEntity, BREEntity>
    {
        public const string StopActivityName = "StopActivity";

        public override bool Compile()
        {
            Console.WriteLine(this.ToString() + "=True");
            return true;
        }
    }
}
