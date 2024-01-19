using System;
namespace BRE.RulesEngine
{
    public interface IRulesEngineProcess<TInput, TOutput>
        where TInput : BRE.Entities.IBREEntity
        where TOutput : BRE.Entities.IBREEntity, new()
    {
        TOutput Compile();
        TOutput Compile(TInput entity);
        object Compile(TInput entity, TOutput outputEntity);
        object Compile(TInput entity, TOutput outputEntity, BRE.Rules.Rule<TInput, TOutput> rule);
        object Compile(TInput entity, TOutput outputEntity, System.Collections.Generic.IEnumerable<BRE.Rules.Rule<TInput, TOutput>> rules);
        TInput Input { get; set; }
        TOutput Output { get; set; }
        string Logs { get; }
        BRE.Rules.RuleSet<TInput, TOutput> RuleSet { get; set; }
        RulesLog<TInput, TOutput> RulesLogs { get; }
        RulesEngineStatus Status { get; set; }
        BRE.Rules.Variables Variables { get; set; }
    }
}
