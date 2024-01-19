using BRE.Entities;
using BRE.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Workflow
{
    public abstract class AbstractWorkflow<TInput, TOutput> : IWorkflow<TInput,TOutput>
        where TInput : IBREEntity
        where TOutput : IBREEntity, new()
    {
        public TInput Input { get; set; }
        public TOutput Output { get; set; }
        public Dictionary<string, IActivity> Dico { get; set; }
        public Variables Variables { get; set; }
        public IActivity CurrentActivity { get; set; }
        public bool IsCurrentActivityStop { get; set; }

        public AbstractWorkflow()
        {
            Dico = new Dictionary<string, IActivity>();
            AddActivity(new StartActivity());
            AddActivity(new StopActivity());
        }

        public IActivity GetActivity(string name)
        {
            CurrentActivity = Dico[name];
            return CurrentActivity;
        }
        public T GetActivity<T>() where T : IActivity
        {
            return (T)GetActivity(typeof(T).Name);
        }

        public StartActivity GetStartActivity()
        {
            return GetActivity<StartActivity>();
        }

        public StopActivity GetStopActivity()
        {
            return GetActivity<StopActivity>();
        }

        public void AddActivity(IActivity activity)
        {
            Dico.Add(activity.Name, activity);
        }

        public virtual bool Start()
        {
            foreach (IActivity a in Dico.Values)
            {
                a.Variables = Variables;
            }
            Console.WriteLine(this.ToString() + ".Start");
            IsCurrentActivityStop = GetStartActivity().Compile();
            bool b = Compile();
            Console.WriteLine(this.ToString() + ".Stop="+b);
            return b;
        }

        protected virtual bool Compile()
        {
            if (IsCurrentActivityStop && CurrentActivity.GetType() != typeof(StopActivity))
                throw new BREException("AbstractWorkflow.Compile.Failed " + CurrentActivity.ToString());
            return IsCurrentActivityStop;
        }

        public override string ToString()
        {
            return GetType().Name + "[" + Dico.Values.Count + "]";
        }

    }
}
