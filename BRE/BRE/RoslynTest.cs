using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Z.Expressions;

namespace BRE
{
    public class RoslynTest
    {
        public string code = @"{
                int res=0;   
                for(int i=0; i<100; i++)
                {
                    res+=i;
                }
                return res;
            }";

        private Func<int, int>? fn;


        public void Test()
        {
            fn = Eval.Compile<Func<int, int>>(code);
            for(int i=0;i<10000;i++)
            {
                var res = fn(2);
                Console.WriteLine(res);
            }
            
        }
    }
}
