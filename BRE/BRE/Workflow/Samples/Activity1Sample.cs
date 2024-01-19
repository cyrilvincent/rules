using BRE.Entities.Sample;
using BRE.Rules.Sample;
using BRE.RulesEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Workflow.Samples
{
    public class Activity1Sample : AbstractActivity<GameEntity, GameOutput>
    {
        public Activity1Sample()
        {
            RuleSet = new SampleGameRuleSet();
        }
    }
}
