using BRE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.RulesEngine
{
    public static class RulesLogFacade
    {
        public static string Logs<TInput, TOutput>(RulesLog<TInput, TOutput> rulesLog) where TInput : IBREEntity where TOutput : IBREEntity {
            string s = string.Empty;
            foreach (RuleLog<TInput, TOutput> log in rulesLog.Logs)
            {
                s += Log<TInput, TOutput>(log) + "\n";
            }
            return s;
        }

        public static string Log<TInput, TOutput>(RuleLog<TInput, TOutput> log)
            where TInput : IBREEntity
            where TOutput : IBREEntity
        {
            string s = log.Rule.ToString() + "(" + log.Input.ToString() + ")";
            return s;
        }
    }
}
