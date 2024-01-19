using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Entities.Sample
{
    public class GameEntity : IBREEntity
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Value { get; set; }
        public int Try { get; set; }
    }
}
