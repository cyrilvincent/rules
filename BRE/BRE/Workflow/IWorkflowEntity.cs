using BRE.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Workflow
{
    public interface IWorkflowEntity
    {
        Variables Variables { get; set; }
    }
}
