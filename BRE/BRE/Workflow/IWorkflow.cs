using BRE.Entities;
using System;
using System.Collections.Generic;
namespace BRE.Workflow
{
    public interface IWorkflow<TInput, TOutput> : IWorkflowEntity
        where TInput : IBREEntity
        where TOutput : IBREEntity, new()
    {
        void AddActivity(IActivity activity);
        Dictionary<string, IActivity> Dico { get; set; }
        IActivity GetActivity(string name);
        TInput Input { get; set; }
        TOutput Output { get; set; }
        bool Start();
    }
}
