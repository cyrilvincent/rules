using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRE.Entities;

namespace BRE.Entities
{
    public class BREException : Exception
    {
        public BREException() : base() { }
        public BREException(string msg) : base(msg)
        {
            Console.WriteLine(msg);
        }
        public BREException(string msg, Exception innerException) : base(msg, innerException)
        {
            Console.WriteLine(msg, innerException);
        }
    }
}
