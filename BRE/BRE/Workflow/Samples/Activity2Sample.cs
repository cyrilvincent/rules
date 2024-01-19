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
    public class Activity2Sample : AbstractActivity<GameOutput, EntityOutput>
    {
        public Activity2Sample()
        {
            Engine = new RulesEngineProcess<GameOutput, EntityOutput>();
            RuleSet = new SampleWorkflowRuleSet();
        }

    }
}
