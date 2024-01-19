using BRE.Entities;
using BRE.Entities.Sample;
using BRE.Rules;
using BRE.Rules.Sample;
using BRE.RulesEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Workflow.Samples
{
    public class WorkflowSample : AbstractWorkflow<GameEntity,EntityOutput>, IWorkflow<GameEntity,EntityOutput>
    {
        public WorkflowSample()
        {
            AddActivity(new Activity1Sample());
            AddActivity(new Activity2Sample());
        }

        protected override bool Compile()
        {
            Activity1Sample activity1 = GetActivity<Activity1Sample>();
            activity1.Input = Input;
            IsCurrentActivityStop = CurrentActivity.Compile();
            if (!IsCurrentActivityStop)
            {
                Activity2Sample activity2 = GetActivity<Activity2Sample>();
                activity2.Input = activity1.Output;
                IsCurrentActivityStop = CurrentActivity.Compile();
                if (!IsCurrentActivityStop)
                {
                    Output = activity2.Output;
                    IsCurrentActivityStop = GetStopActivity().Compile();
                }
            }
            return base.Compile();
        }
    }
}
