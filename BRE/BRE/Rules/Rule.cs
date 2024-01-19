using BRE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Rules
{
    public class Rule<TInput, TOutput> : IRuleEntity where TInput : IBREEntity where TOutput : IBREEntity
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public Func<TInput, Variables, bool> Condition { get; set; }
        public Func<TInput, Variables, bool> WhileCondition { get; set; }
        public Func<TInput, TOutput, Variables, object> Action { get; set; }
        public Func<TInput, TOutput, Variables, object> ElseAction { get; set; }
        public int Priority { get; set; }
        public bool Chained { get; set; }

        public Rule()
        {
            Condition = (r, v) => true;
        }

        public override string ToString()
        {
            return "Rule[" + Name + "[" + Index + "][" + Priority + "]]";
        }
    }
}
