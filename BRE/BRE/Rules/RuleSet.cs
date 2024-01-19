using BRE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Rules
{
    public class RuleSet<TInput, TOutput> : IRuleEntity where TInput : IBREEntity
                                                        where TOutput : IBREEntity
    {
        public List<Rule<TInput, TOutput>> Rules { get; set; }
        public IEnumerable<Rule<TInput, TOutput>> ChainedRules
        {
            get { return Rules.Where(r => r.Chained); }
        }

        private string name = null;
        public string Name
        {
            get
            {
                if (name == null) name = GetType().FullName;
                return name;
            }
            set { name = value; }
        }

        public RuleSet()
        {
            Rules = new List<Rule<TInput, TOutput>>();
        }

        public IEnumerable<Rule<TInput, TOutput>> GetRules(string ruleName)
        {
            return Rules.Where(r => r.Name == ruleName);
        }

        public Rule<TInput, TOutput> GetRule(string ruleName)
        {
            return GetRules(ruleName).FirstOrDefault();
        }
        public Rule<TInput, TOutput> GetRule(int index)
        {
            return Rules.Where(r => r.Index == index).FirstOrDefault();
        }
    }
}
