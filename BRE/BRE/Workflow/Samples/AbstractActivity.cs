using BRE.Entities;
using BRE.RulesEngine;
using BRE.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Workflow
{
    public abstract class AbstractActivity<TInput, TOutput> : IActivity where TInput: IBREEntity where TOutput : IBREEntity, new()
    {
        public string Name { get; set; }
        public IRulesEngineProcess<TInput, TOutput> Engine { get; set; }
        public TInput Input { get; set; }
        public TOutput Output { get; set; }
        public RuleSet<TInput, TOutput> RuleSet { get; set; }
        public Variables Variables { get;set;}
        public AbstractActivity()
        {
            Engine = new ChainedRulesEngineProcess<TInput, TOutput>();
            Name = GetType().Name;
        }

        public virtual bool Compile()
        {
            Engine.RuleSet = RuleSet;
            Engine.Variables = Variables;
            Output = Engine.Compile(Input);
            bool b = Engine.Status != RulesEngineStatus.Success;
            Console.WriteLine(this.ToString() + "=" + b);
            return b;
        }

        public override string ToString()
        {
            return GetType().Name;
        }


    }
}
