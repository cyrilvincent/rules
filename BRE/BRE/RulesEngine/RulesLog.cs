using BRE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.RulesEngine
{
    public class RulesLog<TInput, TOutput> : IRulesEngineEntity
        where TInput : IBREEntity
        where TOutput : IBREEntity
    {
        public List<RuleLog<TInput, TOutput>> Logs { get; set; }

        public RulesLog()
        {
            Logs = new List<RuleLog<TInput, TOutput>>();
        }
    }
}
