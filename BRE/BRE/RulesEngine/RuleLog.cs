using BRE.Rules;
using BRE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.RulesEngine
{
    public class RuleLog<TInput, TOutput> : IRulesEngineEntity where TInput:IBREEntity where TOutput:IBREEntity
    {
        private Rule<TInput, TOutput> rule = null;
        public Rule<TInput, TOutput> Rule
        {
            get { return rule; }
            set
            {
                rule = value;
                string s = RulesLogFacade.Log<TInput, TOutput>(this);
                Console.WriteLine(s);
            }
        }
        public TInput Input {get;set;}
        public DateTime DateTime { get; set; }

        public RuleLog()
        {
            DateTime = DateTime.Now;
        }

    }
}
