using BRE.Entities.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Rules.Sample
{
    public class SampleRuleSet : RuleSet<MyEntity, EntityOutput>
    {
        public SampleRuleSet() {
            Rule<MyEntity, EntityOutput> r = new Rule<MyEntity, EntityOutput>
            {
                Name = "Square",
                Action = (i, o, v) => i.MyInt = i.MyInt * i.MyInt
            };
            Rules.Add(r);
            r = new Rule<MyEntity, EntityOutput>
            {
                Name = "Concat",
                Action = (i, o, v) => i.Text = i.Text + i.MyInt
            };
            Rules.Add(r);
            r = new Rule<MyEntity, EntityOutput>
            {
                Name = "Condition",
                Condition = (e, v) => e.MyInt > 2,
                Action = (i, o, v) => i.Text = i.Text + i.MyInt
            };
            Rules.Add(r);
            r = new Rule<MyEntity, EntityOutput>
            {
                Name = "Matrix",
                Condition = (e, v) => e.MyInt < 10,
                Action = (i, o, v) => i.MyEnum = EnumSample.C
            };
            Rules.Add(r);
            r = new Rule<MyEntity, EntityOutput>
            {
                Name = "Matrix",
                Condition = (e, v) => e.MyInt >= 10,
                Action = (i, o, v) => i.MyEnum = EnumSample.D,
                Priority = 10
            };
            Rules.Add(r);
            r = new Rule<MyEntity, EntityOutput>
            {
                Name = "Matrix",
                Condition = (e, v) => e.MyInt >= 20,
                Action = (i, o, v) => i.MyEnum = EnumSample.E,
                Priority = 20
            };
            Rules.Add(r);
            r = new Rule<MyEntity, EntityOutput>
            {
                Name = "Output",
                Action = (i, o, v) => o.Value += i.Text + i.MyInt
            };
            Rules.Add(r);
            r = new Rule<MyEntity, EntityOutput>
            {
                Name = "Variable",
                Condition = (e, v) => e.MyInt == (int)v["Variable1"],
                Action = (i, o, v) => o.Value = "Variable1=" + v["Variable1"],
                Priority = 30
            };
            Rules.Add(r);
            r = new Rule<MyEntity, EntityOutput>
            {
                Name = "Delegate",
                Action = MyFunction
            };
            Rules.Add(r);
            r = new Rule<MyEntity, EntityOutput>
            {
                Name = "DynamicRule",
                Action = (i, o, v) => this.Rules.Where(ru => ru.Name == "DynamicRule").First().Chained = !this.Rules.Where(ru => ru.Name == "DynamicRule").First().Chained
            };
            Rules.Add(r);
        }

        public object MyFunction(MyEntity i, EntityOutput o, Variables v)
        {
            // do anything
            return null;
        }
    }
  


}
