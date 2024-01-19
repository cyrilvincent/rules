using BRE.RulesEngine;
using BRE.Entities.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Rules.Sample
{
    public class SampleInfiniteRuleSet : RuleSet<GameEntity, GameOutput>
    {
        public SampleInfiniteRuleSet()
        {
            Rule<GameEntity, GameOutput> r = new Rule<GameEntity, GameOutput>
            {
                Name = "Infinite",
                Action = (i, o, v) => true,
                Chained = true
            };
            Rules.Add(r);
        }
    }
  


}
