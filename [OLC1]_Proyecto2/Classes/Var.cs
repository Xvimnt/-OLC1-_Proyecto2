using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1__Proyecto2.Classes
{
    class Var
    {
        private string value, type;
        public Var(string value, string type)
        {
            this.Value = value;
            this.Type = type;
        }

        public string Value { get => value; set => this.value = value; }
        public string Type { get => type; set => type = value; }
    }
}
