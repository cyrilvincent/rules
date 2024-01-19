using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRE.Rules
{
    public class Variables : IRuleEntity
    {
        public Dictionary<string, Variable> Dico { get; set; }

        public string Name
        {
            get { return GetType().FullName; }
            set { }
        }

        public Variables()
        {
            Dico = new Dictionary<string, Variable>();
        }

        public object this[string name]
        {
            get
            {
                object o = null;
                try
                {
                    o = Dico[name].Value;
                }
                catch (KeyNotFoundException e)
                {
                    throw new BRE.Entities.BREException("Variables The variable " + name + " does not exist", e);
                }
                return o;
            }
        }

        public void Add(Variable v)
        {
            Dico.Add(v.Name, v);
        }

        public string SValue(string name)
        {
            return (string)Dico[name].Value;
        }

        public double NValue(string name)
        {
            return (double)Dico[name].Value;
        }

        public T Value<T>(string name)
        {
            return (T)Dico[name].Value;
        }



        public List<string> SList(string name)
        {
            return (List<string>)Dico[name].Value;
        }

        public List<double> NList(string name)
        {
            return (List<double>)Dico[name].Value;
        }

        public List<int> IList(string name)
        {
            return (List<int>)Dico[name].Value;
        }

        public List<T> List<T>(string name)
        {
            return (List<T>)Dico[name].Value;
        }
    }
}
