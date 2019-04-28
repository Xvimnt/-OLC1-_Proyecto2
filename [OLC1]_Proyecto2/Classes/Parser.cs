using Irony.Parsing;

namespace _OLC1__Proyecto2.Classes
{
    class Parser : Grammar
    {
        public Parser() : base(false)
        {
            ////----------------------------------Comments
            CommentTerminal SingleLineComment = new CommentTerminal("SingleLineComment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
            CommentTerminal DelimitedComment = new CommentTerminal("DelimitedComment", "/*", "*/");
            NonGrammarTerminals.Add(SingleLineComment);
            NonGrammarTerminals.Add(DelimitedComment);
            ////----------------------------------Declaring REGEX
            var integer = new RegexBasedTerminal("integer", "[0-9]+");
            var number = new RegexBasedTerminal("number", "[0-9]+(.[0-9]+)");
            var String = TerminalFactory.CreateCSharpString("string");
            var caracter = TerminalFactory.CreateCSharpChar("char");
            var boolean = new RegexBasedTerminal("boolean", "(true)|(false)");
            var iden = TerminalFactory.CreateCSharpIdentifier("id");
            ////----------------------------------Declaring all NonTerminals
            var START = new NonTerminal("START");
            var START2 = new NonTerminal("START2");
            var BODY = new NonTerminal("BODY");
            var DECLARATION = new NonTerminal("DECLARATION");
            var DECLARATION2 = new NonTerminal("DECLARATION2");
            var ASSIGNATION = new NonTerminal("ASSIGNATION");
            var ASSIGN2 = new NonTerminal("ASSIGN2");
            var INDEX = new NonTerminal("INDEX");
            var DATATYPE = new NonTerminal("DATATYPE");
            var OBJECTS = new NonTerminal("OBJECTS");
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
            var WHILE = new NonTerminal("WHILE");
            var VARMANAGMENT = new NonTerminal("VARMANAGMENT");
            var UPDATE = new NonTerminal("UPDATE");
            var ESINGLE = new NonTerminal("ESINGLE");
            var E = new NonTerminal("E");
            var ID = new NonTerminal("ID");
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
            KeyTerm or = ToTerm("!!");
            ////----------------------------------precedence
            this.RegisterOperators(1, Associativity.Left, or);
            this.RegisterOperators(2, Associativity.Left, and);
            this.RegisterOperators(3, Associativity.Left, not);
            this.RegisterOperators(4, Associativity.Left, doubleEqual, different,lessThan,lessThanEqual,greaterThan,greaterThanEqual);
            this.RegisterOperators(5, Associativity.Left, plus, minus);
            this.RegisterOperators(6, Associativity.Left, by, divided);
            this.RegisterOperators(7, Associativity.Left, power);
            //Set the start
            this.Root = START;
            //----------------------------------Grammar
            START.Rule = START2;
            START2.Rule = MakePlusRule(START2, BODY);
            //START2.Rule =  START2 + BODY | BODY;
            BODY.Rule = DECLARATION | ASSIGNATION | UPDATE | PRINT | SHOW | IF | FOR | WHILE;
            ASSIGNATION.Rule =  ID + ASSIGN2 + ";";
            ASSIGN2.Rule =  ToTerm("=") + E | "[" +E +"]" + ASSIGN2;
            DECLARATION.Rule = DATATYPE + DECLARATION2;
            DECLARATION2.Rule = OBJECTS + ";" | "arreglo" + ID + ARRAY + ";";
            DATATYPE.Rule = ToTerm("int") | "bool" | "string" | "double" | "char";
            OBJECTS.Rule = OBJECTS + "," + ID + ASSIGN | ID + ASSIGN;
            ASSIGN.Rule = ToTerm("=") + E | Empty ;
            ARRAY.Rule = ToTerm("[") + E + "]" + ARRAY2;
            ARRAY2.Rule = ToTerm("[") + E + "]" + ARRAY | "=" + ARRAYASIGN | Empty;
            ARRAYASIGN.Rule = ToTerm("{") + ARRAYASIGN2 + "}";
            ARRAYASIGN2.Rule = ARRAYASIGN3 | ARRAYLIST;
            ARRAYASIGN3.Rule = ARRAYASIGN | ARRAYASIGN3 + "," + ARRAYASIGN;
            ARRAYLIST.Rule = ARRAYLIST + "," + E | E;
            NATIVE.Rule = integer | caracter | String | boolean | number;
            PRINT.Rule = ToTerm("imprimir") + "(" + E + ")" + ";";
            SHOW.Rule = ToTerm("show") + "(" + E + "," + E + ")" + ";";
            IF.Rule = ToTerm("si") + "(" + E + ")" + "{" + START2 + "}" + ELSE;
            ELSE.Rule =  ToTerm("sino") + IF | "sino" + "{" + START2 + "}" | Empty;
            FOR.Rule = ToTerm("para") + "(" + VARMANAGMENT + E + ";" + UPDATE + ")" + "{" + START2 + "}";
            WHILE.Rule = ToTerm("repetir") + "(" + E + ")" + "{" + START2 + "}";
            VARMANAGMENT.Rule = DECLARATION | ASSIGNATION;
            UPDATE.Rule =  ESINGLE + "++" | ESINGLE + "--";
            ESINGLE.Rule = NATIVE | ID + INDEX;
            INDEX.Rule = ToTerm("(") + E + ")" | Empty;
            ID.Rule = iden;
            E.Rule = E + "+" + E
            | E + "-" + E
            | E + "*" + E
            | E + "/" + E
            | E + "^" + E
            | E + "==" + E
            | E + "!=" + E
            | E + "<" + E
            | E + ">" + E
            | E + "<=" + E
            | E + ">=" + E
            | E + "|" + E
            | E + "&&" + E
            | ToTerm("!") + E
            | ESINGLE
            | ToTerm("(") + E + ")"
            | ToTerm("-") + E;

        }

    }
}
