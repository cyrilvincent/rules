using BRE.Entities.Sample;
using BRE.Rules.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Workflow.Samples
{
    public class LargeRuleSetActivitySample : AbstractActivity<MyEntity, MyEntity>
    {
        public LargeRuleSetActivitySample()
        {
            RuleSet = new SampleLargeRuleSet();
        }
    }
}
