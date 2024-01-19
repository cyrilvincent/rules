using BRE.Entities;
using BRE.Rules;
using BRE.RulesEngine;
using System;
namespace BRE.Workflow
{
    public interface IActivity : IWorkflowEntity
    {
        string Name { get; set; }
        bool Compile();
    }
}
