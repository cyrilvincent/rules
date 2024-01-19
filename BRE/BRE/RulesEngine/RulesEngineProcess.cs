using BRE.Rules;
using BRE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BRE.RulesEngine
{
    public class RulesEngineProcess<TInput, TOutput> : IRulesEngineProcess<TInput,TOutput>, IRulesEngineEntity where TInput: IBREEntity where TOutput : IBREEntity, new()
    {
        public RuleSet<TInput, TOutput> RuleSet { get; set; }
        public RulesLog<TInput, TOutput> RulesLogs { get; private set; }
        public Variables Variables { get; set; }

        public RulesEngineStatus Status { get; set; }

        public TInput Input { get; set; }
        public TOutput Output { get; set; }

        public string Logs
        {
            get
            {
                return RulesLogFacade.Logs<TInput, TOutput>(RulesLogs);
            }
        }

        public RulesEngineProcess()
        {
            RulesLogs = new RulesLog<TInput, TOutput>();
        }

        public TOutput Compile()
        {
            return Compile(Input);
        }

        public TOutput Compile(TInput entity)
        {
            Status = RulesEngineStatus.Start;
            TOutput output = new();
            Compile(entity, output);
            Output = output;
            return output;
        }

        public virtual object Compile(TInput input, TOutput output)
        {
            var o = Compile(input, output, RuleSet.Rules);
            if (Status != RulesEngineStatus.Failed)
                Status = RulesEngineStatus.Success;
            Output = output;
            return o;
        }

        public virtual object Compile(TInput entity, TOutput outputEntity, IEnumerable<Rule<TInput, TOutput>> rules)
        {
            bool stop = false;
            object? o = null;
            foreach (Rule<TInput, TOutput> rule in rules.OrderBy(r=>r.Priority).ThenBy(r=>r.Index))
            {
                o = Compile(entity, outputEntity, rule);
                if (o != null)
                {
                    RulesEngineStatus? status = o as RulesEngineStatus?;
                    if (status != null)
                    {
                        if ((RulesEngineStatus)status == RulesEngineStatus.Failed)
                        {
                            Status = RulesEngineStatus.Failed;
                            Console.WriteLine("Failed");
                            break;
                        }
                        else if ((RulesEngineStatus)status == RulesEngineStatus.Success)
                        {
                            Status = RulesEngineStatus.Success;
                            Console.WriteLine("Success");
                            break;
                        }
                    }

                }
            }
            return o;
        }

        public object Compile(TInput entity, TOutput outputEntity, Rule<TInput, TOutput> rule)
        {
            object? o = null;
            try
            {
                bool condition = rule.Condition(entity, Variables);
                if (condition)
                {
                    o = rule.Action(entity, outputEntity, Variables);
                    RuleLog<TInput, TOutput> log = new() { Input = entity, Rule = rule };
                    RulesLogs.Logs.Add(log);
                }
                else if(rule.ElseAction != null)
                {
                    o = rule.ElseAction(entity, outputEntity, Variables);
                    RuleLog<TInput, TOutput> log = new() { Input = entity, Rule = rule };
                    RulesLogs.Logs.Add(log);
                }
                else if (rule.WhileCondition != null)
                {
                    while (rule.Condition(entity, Variables))
                    {
                        o = rule.Action(entity, outputEntity, Variables);
                        RuleLog<TInput, TOutput> log = new() { Input = entity, Rule = rule };
                        RulesLogs.Logs.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BREException("RulesEngineProcess.Compile(" + entity + "," + outputEntity + "," + rule + ")", ex);
            }
            return o;
        }
    }
}
