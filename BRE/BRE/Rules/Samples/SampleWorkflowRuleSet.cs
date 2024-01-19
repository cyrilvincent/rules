using BRE.Entities.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Rules.Sample
{
    public class SampleWorkflowRuleSet : RuleSet<GameOutput, EntityOutput>
    {
        public SampleWorkflowRuleSet()
        {
            Rules.Add(new Rule<GameOutput, EntityOutput>
            {
                Name = "Activity2.Concat",
                Action = (i, o, v) => o.Value = string.Format(v["Text"].ToString(), "Toto", i.Value)
            });
        }
    }
}
