using BRE.Entities;
using BRE.RulesEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Workflow
{
    public class SimpleActivity<T> : AbstractActivity<T,T>
        where T : IBREEntity, new()
    {
        public SimpleActivity()
        {
            Engine = new RulesEngineProcess<T, T>();
        }
    }
}
