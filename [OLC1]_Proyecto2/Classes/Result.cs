using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1__Proyecto2.Classes
{
    class Result
    {
        private int line, column;
        private string type, value;
        public Result()
        {

        }
        public Result(int line, int column, string type, string value)
        {
            this.Line = line;
            this.Column = column;
            this.Type = type;
            this.Value = value;
        }

        public int Line { get => line; set => line = value; }
        public int Column { get => column; set => column = value; }
        public string Type { get => type; set => type = value; }
        public string Value { get => value; set => this.value = value; }
    }
}
