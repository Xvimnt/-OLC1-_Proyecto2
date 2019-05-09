using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1__Proyecto2.Classes
{
    class error
    {
        private string value, type, description;
        private int line, column;

        public error(string value, string type, string description, int line, int column)
        {
            this.Value = value;
            this.Type = type;
            this.Description = description;
            this.Line = line;
            this.Column = column;
        }

        public string Value { get => value; set => this.value = value; }
        public string Type { get => type; set => type = value; }
        public string Description { get => description; set => description = value; }
        public int Line { get => line; set => line = value; }
        public int Column { get => column; set => column = value; }
    }
}
