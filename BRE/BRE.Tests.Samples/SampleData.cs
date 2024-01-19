using BRE.Entities;
using BRE.Entities.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Test.Sample
{
    public static class SampleData
    {
        public static List<MyEntity> Data
        {
            get
            {
                List<MyEntity> l = new List<MyEntity>();
                MyEntity e = new MyEntity { MyInt = 1, Text = "A" };
                l.Add(e);
                e = new MyEntity { MyInt = 2, Text = "B" };
                l.Add(e);
                e = new MyEntity { MyInt = 3, Text = "C" };
                l.Add(e);
                e = new MyEntity { MyInt = 4, Text = "D" };
                l.Add(e);
                e = new MyEntity { MyInt = 5, Text = "E" };
                l.Add(e);
                e = new MyEntity { MyInt = 6, Text = "F"};
                l.Add(e);
                return l;
            }
        }
    }
}
