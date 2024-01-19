using BRE.Entities.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Rules.Sample
{
    public class SampleLargeRuleSet : RuleSet<MyEntity, MyEntity>
    {
        public const int Nb = 1000;

        private Random random = new Random((int)DateTime.Now.Ticks);
        public SampleLargeRuleSet()
        {
            for (int j = 0; j < Nb; j++)
            {
                Rule<MyEntity, MyEntity> r = new Rule<MyEntity, MyEntity>
                {
                    Name = "+1",
                    Index = j,
                    Action = (i, o, v) =>
                    {
                        i.MyInt += 1;
                        i.Text += random.Next(10).ToString();
                        return true;
                    }
                };
                Rules.Add(r);
            }
        }
    }
  


}
