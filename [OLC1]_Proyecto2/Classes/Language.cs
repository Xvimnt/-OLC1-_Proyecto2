using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1__Proyecto2.Classes
{
    class Language : Grammar
    {
        public Language()
            : base(false)
        {

            CommentTerminal LINE_COMMENT = new CommentTerminal("LINE_COMMENT", ">>", "\n", "\r\n");
            CommentTerminal BLOCK_COMMENT = new CommentTerminal("BLOCK_COMMENT", "<-", "->");
            NonGrammarTerminals.Add(BLOCK_COMMENT);
            NonGrammarTerminals.Add(LINE_COMMENT);

            ////----------------------------------Terminals with precedence
            KeyTerm increase = ToTerm("++");
            KeyTerm decrease = ToTerm("--");
            KeyTerm plus = ToTerm("+");
            KeyTerm minus = ToTerm("-");
            KeyTerm by = ToTerm("*");
            KeyTerm divided = ToTerm("/");
            KeyTerm power = ToTerm("^");
            KeyTerm lessThan = ToTerm("<");
            KeyTerm greaterThan = ToTerm(">");
            KeyTerm lessThanEqual = ToTerm("<=");
            KeyTerm greaterThanEqual = ToTerm(">=");
            KeyTerm doubleEqual = ToTerm("==");
            KeyTerm different = ToTerm("!=");
            KeyTerm not = ToTerm("!");
            KeyTerm and = ToTerm("&&");
            KeyTerm or = ToTerm("||");
            ////----------------------------------precedence
            this.RegisterOperators(1, Associativity.Left, or);
            this.RegisterOperators(2, Associativity.Left, and);
            this.RegisterOperators(3, Associativity.Left, not);
            this.RegisterOperators(4, Associativity.Left, doubleEqual, different, lessThan, lessThanEqual, greaterThan, greaterThanEqual);
            this.RegisterOperators(5, Associativity.Left, plus, minus);
            this.RegisterOperators(6, Associativity.Left, by, divided);
            this.RegisterOperators(7, Associativity.Left, power);

            //Regex
            var integer = new RegexBasedTerminal("int", "[0-9]+");
            var tdouble = TerminalFactory.CreateCSharpNumber("double");
            var String = TerminalFactory.CreateCSharpString("string");
            var caracter = TerminalFactory.CreateCSharpChar("char");
            var boolean = new RegexBasedTerminal("bool", "(true)|(false)");
            var iden = TerminalFactory.CreateCSharpIdentifier("id");

            //Non terminals
            var START = new NonTerminal("START");
            var BODY = new NonTerminal("BODY");
            var DECLARATION = new NonTerminal("DECLARATION");
            var DECLARATION2 = new NonTerminal("DECLARATION2");
            var ASSIGNATION = new NonTerminal("ASSIGNATION");
            var ASSIGN2 = new NonTerminal("ASSIGN2");
            var INDEX = new NonTerminal("INDEX");
            var DATATYPE = new NonTerminal("DATATYPE");
            var OBJECT = new NonTerminal("OBJECT");
            var ASSIGN = new NonTerminal("ASSIGN");
            var ARRAY = new NonTerminal("ARRAY");
            var ARRAY2 = new NonTerminal("ARRAY2");
            var ARRAYASIGN = new NonTerminal("ARRAYASIGN");
            var ARRAYASIGN2 = new NonTerminal("ARRAYASIGN2");
            var ARRAYASIGN3 = new NonTerminal("ARRAYASIGN3");
            var ARRAYLIST = new NonTerminal("ARRAYLIST");
            var NATIVE = new NonTerminal("NATIVE");
            var PRINT = new NonTerminal("PRINT");
            var SHOW = new NonTerminal("SHOW");
            var IF = new NonTerminal("IF");
            var ELSE = new NonTerminal("ELSE");
            var FOR = new NonTerminal("FOR");
            var REPEAT = new NonTerminal("REPEAT");
            var WHILE = new NonTerminal("WHILE");
            var VARMANAGMENT = new NonTerminal("VARMANAGMENT");
            var UPDATE = new NonTerminal("UPDATE");
            var ESINGLE = new NonTerminal("ESINGLE");
            var E = new NonTerminal("E");
            var ID = new NonTerminal("ID");
            var ARRAYS = new NonTerminal("ARRAYS");
            var DOWHILE = new NonTerminal("DOWHILE");
            var Comprobar = new NonTerminal("Comprobar");
            var Casos = new NonTerminal("Casos");
            var Default = new NonTerminal("Default");
            var Casos2 = new NonTerminal("Casos2");
            ////----------------------------------Innecesary nodes
            this.MarkPunctuation("(", ")", "{", "}", "[", "]", ";", "=", ",","si","para","repetir","mientras","show","hacer");
            this.MarkTransient(BODY, ASSIGN2, DECLARATION2, ARRAY2,ARRAYASIGN, ARRAYASIGN2, ARRAYASIGN3, NATIVE, VARMANAGMENT,ESINGLE, ASSIGN,ARRAY);
            //----------------------------------Grammar
            START.Rule = MakePlusRule(START, BODY);
            BODY.Rule = DECLARATION | ASSIGNATION | UPDATE + ";" | PRINT | SHOW | IF | FOR | REPEAT | WHILE | DOWHILE;
            //Body declaration and functions
            DECLARATION.Rule = DATATYPE + DECLARATION2;
            DECLARATION2.Rule = OBJECT + ";" | ToTerm("arreglo") + ARRAYS + ";";
            ARRAYS.Rule = ID + ARRAY;
            ASSIGN.Rule = ToTerm("=") + E | Empty;
            ASSIGNATION.Rule = ID + ASSIGN2 + ";";
            ASSIGN2.Rule = ToTerm("=") + E | "[" + E + "]" + ASSIGN2;
            PRINT.Rule = ToTerm("print") + "(" + E + ")" + ";";
            SHOW.Rule = ToTerm("show") + "(" + E + "," + E + ")" + ";";
            IF.Rule = ToTerm("si") + "(" + E + ")" + "{" + START + "}" + ELSE;
            ELSE.Rule = ToTerm("sino") + IF | ToTerm("sino") + "{" + START + "}" | Empty;
            FOR.Rule = ToTerm("para") + "(" + VARMANAGMENT + E + ";" + UPDATE + ")" + "{" + START + "}";
            REPEAT.Rule = ToTerm("repetir") + "(" + E + ")" + "{" + START + "}";
            VARMANAGMENT.Rule = DECLARATION | ASSIGNATION;
            UPDATE.Rule = ESINGLE + increase  | ESINGLE + decrease ;
            WHILE.Rule = ToTerm("mientras") + "(" + E + ")" + "{" + START + "}";
            DOWHILE.Rule = ToTerm("hacer") + "{" + START + "}" + ToTerm("mientras") + "(" + E + ")" + ";";
            DOWHILE.ErrorRule = SyntaxError + "}";
            DOWHILE.ErrorRule = SyntaxError + ";";
            Comprobar.Rule = ToTerm("comprobar") + "(" + E + ")" + "{" + Casos + Default + "}";
            Comprobar.ErrorRule = SyntaxError + "}";
            Comprobar.ErrorRule = SyntaxError + ";";
            Casos.Rule = Casos2 + ToTerm("caso") + E + ":" + START + ToTerm("salir") + ";";
            Casos2.Rule = Empty | Casos2 + ToTerm("caso") + E + ":" + START + ToTerm("salir") + ";";
            Default.Rule = ToTerm("defecto") + ":" + START + ToTerm("salir") + ";" | Empty;

            //datatypes 
            DATATYPE.Rule = ToTerm("int") | "bool" | "string" | "double" | "char";
            OBJECT.Rule = OBJECT + "," + ID + ASSIGN | ID + ASSIGN;
            //Making arrays
            ARRAY.Rule = "=" + ARRAYASIGN | Empty;
            ARRAYASIGN.Rule = ToTerm("{") + ARRAYASIGN2 + "}";
            ARRAYASIGN2.Rule = ARRAYASIGN3 | ARRAYLIST;
            ARRAYASIGN3.Rule = ARRAYASIGN | MakePlusRule(ARRAYASIGN3,ToTerm(","), ARRAYASIGN);
            ARRAYLIST.Rule = MakePlusRule(ARRAYLIST,ToTerm(","),E);
            //Making EXP
            E.Rule = E + plus + E
            | E + minus + E
            | E + by + E
            | E + divided + E
            | E + power + E
            | E + doubleEqual + E
            | E + different + E
            | E + lessThan + E
            | E + greaterThan + E
            | E + lessThanEqual + E
            | E + greaterThanEqual + E
            | E + or + E
            | E + and + E
            | not + E
            | ESINGLE
            | ToTerm("(") + E + ")"
            | minus + E;
            ESINGLE.Rule = NATIVE | ID;
            INDEX.Rule = INDEX + ToTerm("[") + E + "]" | Empty;
            ID.Rule = iden + INDEX;
            NATIVE.Rule = integer | caracter | String | boolean | tdouble;


            this.Root = START;
        }
    }
}

