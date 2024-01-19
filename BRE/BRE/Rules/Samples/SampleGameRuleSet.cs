using BRE.RulesEngine;
using BRE.Entities.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Rules.Sample
{
    public class SampleGameRuleSet : RuleSet<GameEntity, GameOutput>
    {
        public SampleGameRuleSet()
        {
            Rule<GameEntity, GameOutput> r = new Rule<GameEntity, GameOutput>
            {
                Name = "Success",
                Condition = (e, v) => e.Try == e.Value,
                Action = (i, o, v) => {
                    o.Value = i.Try;
                    return RulesEngineStatus.Success;
                },
                Priority = 10,
                Chained = true
            };
            Rules.Add(r);
            r = new Rule<GameEntity, GameOutput>
            {
                Name = "Less",
                Condition = (e, v) => e.Try < e.Value,
                Action = (i, o, v) => {
                    int next = (int)((i.Try + i.Max) / 2);
                    if (i.Try == next) next--;
                    i.Min = i.Try;
                    i.Try = next;
                    return true;
                },
                Priority = 20,
                Chained = true
            };
            Rules.Add(r);
            r = new Rule<GameEntity, GameOutput>
            {
                Name = "Greater",
                Condition = (e, v) => e.Try > e.Value,
                Action = (i, o, v) =>
                {
                    int next = (int)((i.Min + i.Try) / 2);
                    if (i.Try == next) next++;
                    i.Max = i.Try;
                    i.Try = next;
                    return true;
                },
                Priority = 30,
                Chained = true
            };
            Rules.Add(r);
            r = new Rule<GameEntity, GameOutput>
            {
                Name = "Fail",
                Condition = (e, v) => e.Try > e.Max || e.Try < e.Min,
                Action = (i, o, v) =>  RulesEngineStatus.Failed,
                Priority = 0,
                Chained = true
            };
            Rules.Add(r);
            r = new Rule<GameEntity, GameOutput>
            {
                Name = "Fail2",
                Condition = (e, v) => e.Min > e.Max,
                Action = (i, o, v) => RulesEngineStatus.Failed,
                Priority = 1,
                Chained = true
            };
            Rules.Add(r);
        }
    }
  


}
