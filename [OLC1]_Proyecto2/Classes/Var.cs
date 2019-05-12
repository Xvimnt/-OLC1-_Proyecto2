using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1__Proyecto2.Classes
{
    class Var
    {
        private string value, type, visibility;
        private int row, column;
        private ParseTreeNode instructions;

        public Var()
        {
        }

        public Var(string value, string type, string visibility)
        {
            this.Value = value;
            this.Type = type;
            this.Visibility = visibility;
        }

        public Var(string value, string type, string visibility, ParseTreeNode instructions, int row, int column) : this(value, type, visibility, instructions)
        {
            Row = row;
            Column = column;
        }

        public Var(string value, string type, string visibility, ParseTreeNode instructions) : this(value, type, visibility)
        {
            this.Instructions = instructions;
        }

        public string Value { get => value; set => this.value = value; }
        public string Type { get => type; set => type = value; }
        public string Visibility { get => visibility; set => visibility = value; }
        public ParseTreeNode Instructions { get => instructions; set => instructions = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
    }
}
