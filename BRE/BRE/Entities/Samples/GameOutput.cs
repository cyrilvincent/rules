using BRE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Entities.Sample
{
    public class GameOutput : IBREEntity
    {
        //public int Iteration { get; set; }
        //public bool Success { get; set; }
        //public bool Fail { get; set; }
        public object Value { get; set; }
    }
}
