using BRE.Rules;
using BRE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.RulesEngine
{
    public class ChainedRulesEngineProcess<TInput, TOutput> : RulesEngineProcess<TInput, TOutput>
        where TInput : IBREEntity
        where TOutput : IBREEntity, new()
    {
        public int NbIterationMax { get; set; }
        public int Iteration { get; set; }

        public ChainedRulesEngineProcess()
        {
            NbIterationMax = 100;
        }

        public override object Compile(TInput entity, TOutput outputEntity)
        {
            Iteration = 0;
            return base.Compile(entity, outputEntity);
        }

        public override object Compile(TInput entity, TOutput outputEntity, IEnumerable<Rule<TInput, TOutput>> rules)
        {
            object o = base.Compile(entity, outputEntity, rules);
            List<Rule<TInput, TOutput>> nextRules = RuleSet.ChainedRules.ToList();
            if (nextRules.Count > 0 && Status != RulesEngineStatus.Failed && Status != RulesEngineStatus.Success)
            {
                Iteration++;
                Rule<TInput, TOutput> rule = new Rule<TInput, TOutput> { Name = "ChainedRulesEngineProcess.Iteration", Priority = Iteration };
                RulesLogs.Logs.Add(new RuleLog<TInput, TOutput> { Input = entity, Rule = rule });
                if (Iteration < NbIterationMax)
                {
                    o = Compile(entity, outputEntity, nextRules);
                }
                else
                {
                    Status = RulesEngineStatus.Failed;
                    Console.WriteLine(rule.Name + "(" + Iteration + ")");
                }
            }
            return o;
        }
    }
}
