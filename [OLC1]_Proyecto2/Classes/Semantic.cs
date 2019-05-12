using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace _OLC1__Proyecto2.Classes
{
    class Semantic
    {
        // RAMA DE JAVIER
        private string console = "";
        private string currentType, currentID;
        private Dictionary<string,Var> variables = new Dictionary<string, Var>();
        private List<error> errores = new List<error>();
        private List<string> shows = new List<string>();
        public List<Figuras> listafiguras = new List<Figuras>();
        public List<List<Figuras>> ListadeLista = new List<List<Figuras>>();



        public string Console { get => console; set => console = value; }
        internal Dictionary<string, Var> Variables { get => variables; set => variables = value; }
        internal List<error> Errores { get => errores; set => errores = value; }
        public List<string> Shows { get => shows; set => shows = value; }
        
      

        public List<List<Figuras>> getListaLista() // metodo que llamo desde el semantico para obtener las figuras
        {
            return ListadeLista;
        }

        private bool comprobeTypes(Result response)
        {
            return true;
            if (response.Value == "null" || string.IsNullOrWhiteSpace(response.Value)) return true; ;
            switch (currentType)
            {
                case "double":
                    if (response.Type != "double" && response.Type != "int")
                    {
                        var val = currentType + " no es el mismo que " + response.Type + "<" + response.Value + ">";
                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                        return false;
                    }
                    break;
                default:
                    if (response.Type != currentType)
                    {
                        var val = currentType + " no es el mismo que " + response.Type + "<" + response.Value + ">";
                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                        return false;
                    }
                    break;
            }
            return true;
        }

        private void addVariable(Result variable)
        {
            if (!Variables.ContainsKey(currentID))
            {
                if (comprobeTypes(variable))
                {
                    Variables.Add(currentID, new Var(variable.Value, currentType));
                }
            }
            else
            {
                var current = variables[currentID];
                currentType = current.Type;
                if (comprobeTypes(variable))
                {
                    current.Value = variable.Value;
                    current.Type = variable.Type;
                }
            }
            
        }

        public Result execute(ParseTreeNode node_)
        {
            Result response = new Result();
            if (node_ == null) return response;
            // Childrens Array for each node
            ParseTreeNode[] hijos = hijos = node_.ChildNodes.ToArray();
            // Nos servirán para una posible reporte de error de Types.
            response.Line = node_.Span.Location.Line;
            response.Column = node_.Span.Location.Column;
            switch (node_.Term.Name)
            {
                case "DECLARATION":
                    {
                        //To obtain the Type of the iden
                        execute(hijos[0]);
                        Result Obj = execute(hijos[1]);
                    }
                    break;
                case "DATATYPE":
                    {
                        currentType = hijos[0].Token.ValueString.ToLower();
                    }
                    break;
                case "PRINT":
                    {
                        Result op1 = execute(hijos[0]);
                        Console += op1.Value + "\n";
                    }
                    break;
                case "IF":
                    {
                        Result op1 = execute(hijos[0]);
                        Result op2;
                        if (op1.Value == "true")
                        {
                            op2 = execute(hijos[1]);
                        }
                        else if (op1.Value == "false")
                        {
                            if ((hijos.Length > 2) && (hijos[2] != null))
                            {
                                op2 = execute(hijos[2]);
                            }
                        }
                        else
                        {
                            string val = op1.Value + " no es un booleano";
                            Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                        }
                    }
                    break;
                case "SWITCH":
                    {

                        Result op1 = execute(hijos[0]);
                        var cases = hijos[1].ChildNodes.ToArray();
                        foreach (var Case in cases)
                        {
                            var caseChild = Case.ChildNodes.ToArray();
                            if (op1.Value == execute(caseChild[0]).Value)
                            {
                                execute(caseChild[1]);
                                return null;
                            }
                        }
                        var defChild = hijos[2].ChildNodes.ToArray();
                        if (defChild.Length != 0)
                        {
                            execute(defChild[0]);
                        }
                    }
                    break;
                case"FIGURE":
                    {
                        //System.Console.WriteLine(execute(hijos[1]).Value);
                        ListadeLista.Add(new List<Figuras>(listafiguras));
                        listafiguras.Clear();
                        

                    }
                    break;
                case "GEOMETRICAS":
                    { 
                    switch (hijos[0].Token.Value)
                    {
                        case "circle":
                                {

                                    String tipo = hijos[0].Token.Value.ToString();
                                    String Color = execute(hijos[1]).Value;
                                    String radio = execute(hijos[2]).Value;
                                    String solido = execute(hijos[3]).Value;
                                    String posx = execute(hijos[4]).Value;
                                    String posy = execute(hijos[5]).Value;
                                    System.Console.WriteLine(tipo+ "- "+Color + " -" + posx );
                                listafiguras.Add(new Figuras(tipo, Color, int.Parse(posx), int.Parse(posy), int.Parse(radio), int.Parse(radio), 0, 0, 1, solido));
                                // LISTO
                            }
                            break;
                        case "triangle":
                            {
                                String tipo2 = hijos[0].Token.Value.ToString();
                                String Color2 = execute(hijos[1]).Value;
                                String solido2 = execute(hijos[2]).Value;
                                String pt1x = execute(hijos[3]).Value;
                                String pt1y = execute(hijos[4]).Value;
                                String pt2x = execute(hijos[5]).Value;
                                String pt2y = execute(hijos[6]).Value;
                                String pt3x = execute(hijos[7]).Value;
                                String pt3y = execute(hijos[8]).Value;
                                listafiguras.Add(new Figuras(tipo2, Color2, int.Parse(pt1x), int.Parse(pt1y), int.Parse(pt2x), int.Parse(pt2y), int.Parse(pt3x), int.Parse(pt3y), 2, solido2));
                            }
                            break;
                        case "line":
                                {
                                    System.Console.WriteLine("dentr liena");
                                    String tipo3 = hijos[0].Token.Value.ToString();
                                String Color3 = execute(hijos[1]).Value;
                                String inix1 = execute(hijos[2]).Value;
                                String iniy1 = execute(hijos[3]).Value;
                                String finx = execute(hijos[4]).Value;
                                String finy = execute(hijos[5]).Value;
                                String grosor = execute(hijos[6]).Value;
                                listafiguras.Add(new Figuras(tipo3, Color3, int.Parse(inix1), int.Parse(iniy1), int.Parse(finx), int.Parse(finy), 0, 0, int.Parse(grosor), "false"));
                            }
                            break;
                        case "square":
                            {
                                String tipo4 = hijos[0].Token.Value.ToString();
                                String Color4 = execute(hijos[1]).Value;
                                String solido4 = execute(hijos[2]).Value;
                                String xi = execute(hijos[3]).Value;
                                String yi = execute(hijos[4]).Value;
                                String w = execute(hijos[5]).Value;
                                String h = execute(hijos[6]).Value;
                                listafiguras.Add(new Figuras(tipo4, Color4, int.Parse(xi), int.Parse(yi), int.Parse(w), int.Parse(h), 0, 0, 2, solido4));
                            }
                            break;
                    }
            }
                    break;

                               
                case "UPDATE":
                    {
                        var childrens = hijos[0].ChildNodes.ToArray();
                        var op1 = variables[childrens[0].Token.ValueString];
                        string operador = hijos[1].Token.ValueString;
                        switch (operador)
                        {
                            case "++":
                                {
                                    switch (op1.Type)
                                    {
                                        //si es iden tengo que actualizarlo
                                        case "int":
                                            {
                                                response.Type = "int";
                                                int result = int.Parse(op1.Value);
                                                result++;
                                                response.Value = String.Concat(result).ToLower();
                                            }
                                            break;
                                        case "char":
                                            {
                                                response.Type = "int";
                                                int result = char.Parse(op1.Value);
                                                result++;
                                                response.Value = String.Concat(result).ToLower();
                                            }
                                            break;
                                        case "double":
                                            {
                                                response.Type = "double";
                                                double result = double.Parse(op1.Value);
                                                result++;
                                                response.Value = String.Concat(result).ToLower();
                                            }
                                            break;
                                        default:
                                            {
                                                string val = op1.Value + "++";
                                                Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "--":
                                {
                                    switch (op1.Type)
                                    {
                                        case "int":
                                            {
                                                response.Type = "int";
                                                int result = int.Parse(op1.Value);
                                                result--;
                                                response.Value = String.Concat(result).ToLower();
                                            }
                                            break;
                                        case "char":
                                            {
                                                response.Type = "int";
                                                int result = char.Parse(op1.Value);
                                                result--;
                                                response.Value = String.Concat(result).ToLower();
                                            }
                                            break;
                                        case "double":
                                            {
                                                response.Type = "double";
                                                double result = double.Parse(op1.Value);
                                                result--;
                                                response.Value = String.Concat(result).ToLower();
                                            }
                                            break;
                                        default:
                                            {
                                                string val = op1.Value + "--";
                                                Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                        variables[childrens[0].Token.ValueString] = new Var(response.Value,response.Type);
                    }
                    break;
                case "OBJECT":
                    {
                        switch (hijos.Length)
                        {
                            case 1:
                                //ADD TO VARIABLES WITH VALUE NULL
                                execute(hijos[0]);
                                response.Value = "null";
                                addVariable(response);
                                break;
                            case 2:
                                //One line declaration
                                //to save the id in a global variable
                                execute(hijos[0]);
                                addVariable(execute(hijos[1]));
                                break;
                            case 3:
                                //multi line declaration
                                //to save the id in a global variable
                                execute(hijos[1]);
                                addVariable(execute(hijos[2]));
                                execute(hijos[0]);
                                break;
                        }
                    }
                    break;
                case "ID":
                    //to save the current variable
                    currentID = hijos[0].Token.ValueString;

                    //in case is an array
                    response = execute(hijos[1]);
                    break;
                case "INDEX":
                    if(hijos.Length > 0)
                    {
                        Result dim = execute(hijos[0]);
                        var lastDim = execute(hijos[1]);
                        if (!string.IsNullOrWhiteSpace(dim.Value))
                        {
                            response.Value = dim.Value + "," + lastDim.Value;
                        }
                        else
                        {
                            response.Value = lastDim.Value;
                        }
                        response.Type = "dims";
                    }
                    else
                    {
                        response.Value = "";
                    }
                    break;
                case "FOR":
                    {
                        //initialize the variable
                        execute(hijos[0]);
                        Result boolean = execute(hijos[1]);

                        while (boolean.Value == "true")
                        {
                            //Recorriendo el ciclo
                            execute(hijos[3]);
                            //Actualizando el value del iterador
                            execute(hijos[2]);
                            boolean = execute(hijos[1]);
                        }
                    }
                    break;
                case "ASSIGNATION":
                    {
                        //Este es un identificador
                        var index = execute(hijos[0]);
                        var id = currentID;
                        if(!string.IsNullOrWhiteSpace(index.Value))
                        {
                            var data = index.Value.Split(',');
                            foreach(var element in data)
                            {
                                id += "[" + element + "]";
                            }
                            currentID = id;
                        }
                        //asignando variables
                        addVariable(execute(hijos[1]));
                    }
                    break;
                case "REPEAT":
                    {

                        Result times = execute(hijos[0]);
                        if (times.Type != "int")
                        {
                            //Si el numero de veces no es entero entonces es error semantico
                            string val = times.Value + " no es un entero";
                            Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                        }
                        else
                        {
                            int end = int.Parse(times.Value);
                            for (int i = 0; i < end; i++)
                            {
                                //Recorriendo el ciclo
                                execute(hijos[1]);
                            }
                        }
                    }
                    break;
                case "WHILE":
                    {

                        Result boolean = execute(hijos[0]);
                        if (boolean.Type != "bool")
                        {
                            //Si el numero de veces no es entero entonces es error semantico
                            string val = boolean.Value + " no es un booleano";
                            Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                        }
                        else
                        {
                            bool cond = bool.Parse(boolean.Value);
                            while (cond)
                            {
                                execute(hijos[1]);
                                cond = bool.Parse(execute(hijos[0]).Value);
                            }
                        }
                    }
                    break;
                case "DOWHILE":
                    {
                        Result boolean = execute(hijos[1]);
                        if (boolean.Type != "bool")
                        {
                            //Si el numero de veces no es entero entonces es error semantico
                            string val = boolean.Value + " no es un booleano";
                            Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                        }
                        else
                        {
                            bool cond = bool.Parse(boolean.Value);
                            do
                            {
                                execute(hijos[0]);
                                cond = bool.Parse(execute(hijos[1]).Value);
                            } while (cond);
                        }
                    }
                    break;
                case "SHOW":
                    {
                        Result title = execute(hijos[0]);
                        Result msg = execute(hijos[1]);
                        Shows.Add(title.Value + "@" + msg.Value);
                    }
                    break;
                case "ARRAYASIGN3":
                    int count = 0;
                    foreach(var dim in hijos)
                    {
                        //dim is an arraylist or an arrayasign3
                        var temp = currentID;
                        currentID += "[" + count + "]";
                        execute(dim);
                        currentID = temp;
                        count++;
                    }
                    break;
                case "ARRAYLIST":
                    count = 0;
                    foreach(var exp in hijos)
                    {
                        var temp = currentID;
                        currentID += "[" + count + "]";
                        addVariable(execute(exp));
                        currentID = temp;
                        count++;
                    }

                    break;
                case "ARRAYS":
                    {
                        //obtain an int array with the limits of the current array
                        var limits = execute(hijos[0]).Value.Split(',');

                        if(hijos.Length > 1)
                        {
                            // all the array lists nodes or ARRAYASIGN3
                            execute(hijos[1]);
                        }
                        else
                        {
                            // initialize all the indexes of the array with null value
                            switch (limits.Length)
                            {
                                //one dimension array
                                case 1:
                                    var limit = int.Parse(limits[0]);
                                    for (int i = 0; i < limit; i++)
                                    {
                                        var temp = currentID;
                                        currentID += "[" + i + "]";
                                        response.Value = "null";
                                        addVariable(response);
                                        currentID = temp;
                                    }
                                    break;
                                //two dimension array
                                case 2:
                                    limit = int.Parse(limits[0]);
                                    var limit2 = int.Parse(limits[1]);
                                    for (int i = 0; i < limit; i++)
                                    {
                                        for (int j = 0; j < limit2; j++)
                                        {
                                            var temp = currentID;
                                            currentID += "[" + i + "]" + "[" + j + "]";
                                            response.Value = "null";
                                            addVariable(response);
                                            currentID = temp;
                                        }
                                    }
                                    break;
                                //tre dimension array
                                case 3:
                                    limit = int.Parse(limits[0]);
                                    limit2 = int.Parse(limits[1]);
                                    var limit3 = int.Parse(limits[2]);
                                    for (int i = 0; i < limit; i++)
                                    {
                                        for (int j = 0; j < limit2; j++)
                                        {
                                            for (int k = 0; k < limit3; k++)
                                            {
                                                var temp = currentID;
                                                currentID += "[" + i + "]" + "[" + j + "]" + "[" + k + "]";
                                                response.Value = "null";
                                                addVariable(response);
                                                currentID = temp;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "E":
                    {
                        switch (hijos.Length)
                        {
                            case 1:
                                //si es un parentesis
                                if (hijos[0].Term.Name == "E")
                                {
                                    response = execute(hijos[0]);
                                }
                                else if(hijos[0].Term.Name == "ID")
                                {
                                    var childrens = hijos[0].ChildNodes.ToArray();
                                    var id = childrens[0].Token.ValueString;
                                    //to calculate the index
                                    var dims = execute(childrens[1]);
                                    if (!string.IsNullOrWhiteSpace(dims.Value))
                                    {
                                        var data = dims.Value.Split(',');
                                        foreach(var element in data)
                                        {
                                            id += "[" + element + "]";
                                        }
                                    }
                                    var iden = variables[id];
                                    response.Value = iden.Value;
                                    response.Type = iden.Type;
                                }
                       
                                else
                                {
                                    response.Type = hijos[0].Term.Name;
                                    response.Value = hijos[0].Token.ValueString;
                                }
                                break;
                            case 2:
                                var op1 = execute(hijos[1]);
                                string operador = hijos[0].Token.ValueString;
                                switch (operador)
                                {
                                    case "-":
                                        {
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        response.Type = "int";
                                                        int result = int.Parse(op1.Value) * -1;
                                                        response.Value = String.Concat(result).ToLower();
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        response.Type = "double";
                                                        double result = double.Parse(op1.Value) * -1;
                                                        response.Value = String.Concat(result).ToLower();
                                                    }
                                                    break;
                                                default:
                                                    {
                                                        string val = "-" + op1.Value;
                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                }
                                break;
                            case 3:
                                op1 = execute(hijos[0]);
                                operador = hijos[1].Token.ValueString;
                                switch (operador)
                                {
                                    case "==":
                                        {
                                            var op2 = execute(hijos[2]);
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (op1.Value == op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "double":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (op1.Value == op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "string":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "string":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (op1.Value== op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "char":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "char":
                                                                {
                                                                    response.Type = "bool";
                                                                    int c = char.Parse(op2.Value);
                                                                    int a = char.Parse(op1.Value);
                                                                    bool result = (a == c);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "bool":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "bool":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = op1.Value == op2.Value;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "!=":
                                        {
                                            var op2 = execute(hijos[2]);
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (op1.Value != op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "double":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (op1.Value != op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "string":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "string":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (op1.Value!= op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "char":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "char":
                                                                {
                                                                    response.Type = "bool";
                                                                    int c = char.Parse(op2.Value);
                                                                    int a = char.Parse(op1.Value);
                                                                    bool result = (a != c);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "bool":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "bool":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = op1.Value!= op2.Value;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "<":
                                        {
                                            var op2 = execute(hijos[2]);
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (int.Parse(op1.Value) < int.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (double.Parse(op1.Value) < double.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (int.Parse(op1.Value) < int.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (double.Parse(op1.Value) < double.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    {
                                                        string val = op1.Value+ " " + op2.Value;
                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "&&":
                                        {
                                            switch (op1.Type)
                                            {
                                                case "bool":
                                                    {
                                                        //if the first is true continue otherwise no
                                                        if (bool.Parse(op1.Value))
                                                        {
                                                            var op2 = execute(hijos[2]);
                                                            switch (op2.Type)
                                                            {
                                                                case "bool":
                                                                    {
                                                                        response.Type = "bool";
                                                                        bool result = bool.Parse(op1.Value) && bool.Parse(op2.Value);
                                                                        response.Value = String.Concat(result).ToLower();
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        string val = op2.Value + " no es un booleano";
                                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                    }
                                                                    break;
                                                            }
                                                        }

                                                    }
                                                    break;
                                                default:
                                                    {
                                                        string val = op1.Value + " no es un booleano";
                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "||":
                                        {
                                            switch (op1.Type)
                                            {
                                                case "bool":
                                                    {
                                                        if (!bool.Parse(op1.Value))
                                                        {
                                                            var op2 = execute(hijos[2]);
                                                            switch (op2.Type)
                                                            {
                                                                case "bool":
                                                                    {
                                                                        response.Type = "bool";
                                                                        bool result = bool.Parse(op1.Value) || bool.Parse(op2.Value);
                                                                        response.Value = String.Concat(result).ToLower();
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        string val = op2.Value + " no es un booleano";
                                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            response.Type = "bool";
                                                            response.Value = "true";
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    {
                                                        string val = op1.Value+ " no es un booleano";
                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case ">":
                                        {
                                           var op2 = execute(hijos[2]);
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (int.Parse(op1.Value) > int.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (double.Parse(op1.Value) > double.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (double.Parse(op1.Value) > int.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (double.Parse(op1.Value) > double.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    {
                                                        string val = op1.Value+ " " + op2.Value;
                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "<=":
                                        {
                                           var op2 = execute(hijos[2]);
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (int.Parse(op1.Value) <= int.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (int.Parse(op1.Value) <= double.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (double.Parse(op1.Value) <= int.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (double.Parse(op1.Value) <= double.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    {
                                                        string val = op1.Value+ " " + op2.Value;
                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case ">=":
                                        {
                                           var op2 = execute(hijos[2]);
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (int.Parse(op1.Value) >= int.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (int.Parse(op1.Value) >= double.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (double.Parse(op1.Value) >= int.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = (double.Parse(op1.Value) >= double.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    {
                                                        string val = op1.Value+ " " + op2.Value;
                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    
                                    case "+":
                                        {
                                            var op2 = execute(hijos[2]);
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "int";
                                                                    int result = int.Parse(op1.Value) + int.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) + double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "string":
                                                                {
                                                                    response.Type = "string";
                                                                    response.Value= op1.Value+ op2.Value;
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "int";
                                                                    int c = char.Parse(op2.Value);
                                                                    double result = int.Parse(op1.Value) + c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "int";
                                                                    double result;
                                                                    if(op2.Value == "true")
                                                                    {
                                                                        result = int.Parse(op1.Value) + 1;
                                                                    }
                                                                    else
                                                                    {
                                                                        result = int.Parse(op1.Value);
                                                                    }
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) + double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) + double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "double";
                                                                    int boolean;
                                                                    if (op2.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    else boolean = 0;
                                                                    double result = double.Parse(op1.Value) + boolean;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "string":
                                                                {
                                                                    response.Type = "string";
                                                                    response.Value= op1.Value+ op2.Value;
                                                                }
                                                                break;

                                                            case "char":
                                                                {
                                                                    response.Type = "double";
                                                                    int c = char.Parse(op2.Value);
                                                                    double result = double.Parse(op1.Value) + c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;

                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "string":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "string";
                                                                    response.Value= op1.Value+ op2.Value;
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "string";
                                                                    response.Value= op1.Value + op2.Value;
                                                                }
                                                                break;
                                                            case "string":
                                                                {
                                                                    response.Type = "string";
                                                                    response.Value= op1.Value+ op2.Value;
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "string";
                                                                    response.Value= op1.Value+ op2.Value.Replace("'","");
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "char":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "int";
                                                                    int c = char.Parse(op2.Value);
                                                                    double result = c + int.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "int";
                                                                    int a = char.Parse(op1.Value);
                                                                    int c = char.Parse(op2.Value);
                                                                    double result = a + c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "int";
                                                                    int c = char.Parse(op1.Value);
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    int result = c + boolean;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    int c = char.Parse(op1.Value);
                                                                    double result = c + double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "string":
                                                                {
                                                                    response.Type = "string";
                                                                    response.Value= op1.Value.Replace("'","") + op2.Value;
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "bool":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "int";
                                                                    int boolean;
                                                                    if (op2.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    else boolean = 0;
                                                                    double result = boolean + int.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "int";
                                                                    int c = char.Parse(op2.Value);
                                                                    int boolean = 0;
                                                                    if(op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    int result = boolean + c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    
                                                                    response.Type = "bool";
                                                                    if(op1.Value == "true")
                                                                    {
                                                                        if(op2.Value == "true")
                                                                        {
                                                                            response.Value = String.Concat(1);
                                                                        }
                                                                    }
                                                                   
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) + double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "-":
                                        {
                                            var op2 = execute(hijos[2]);
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "int";
                                                                    int result = int.Parse(op1.Value) - int.Parse(op2.Value);
                                                                    response.Value = String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) - double.Parse(op2.Value);
                                                                    response.Value = String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "int";
                                                                    char c = char.Parse(op2.Value);
                                                                    double result = int.Parse(op1.Value) - c;
                                                                    response.Value = String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "int";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result =int.Parse(op1.Value) - boolean;
                                                                    response.Value = String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value + " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) - int.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) - double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "double";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result = double.Parse(op1.Value) - boolean;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "double";
                                                        
                                                                    double result = double.Parse(op1.Value) - char.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "string":
                                                    {

                                                        string val = op1.Value+ " " + op2.Value;
                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                    }
                                                    break;
                                                case "char":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "int";
                                                                    char c = char.Parse(op2.Value);
                                                                    double result = c - int.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "int";
                                                                    char c = char.Parse(op2.Value);
                                                                    int a = char.Parse(op1.Value);
                                                                    double result = a - c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    char c = char.Parse(op2.Value);
                                                                    double result = c - double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "bool":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "int";
                                                                    int boolean = 0;
                                                                    if(op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result = boolean - int.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result = boolean - double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "*":
                                        {
                                           var op2 = execute(hijos[2]);
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "int";
                                                                    int result = int.Parse(op1.Value) * int.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = int.Parse(op1.Value) * double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "int";
                                                                    int c = char.Parse(op2.Value);
                                                                    double result = int.Parse(op1.Value) * c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "int";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result = int.Parse(op1.Value) * boolean;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) * int.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) * double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "double";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result = double.Parse(op1.Value) * boolean;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "double";
                                                                    int c = char.Parse(op2.Value);
                                                                    double result = double.Parse(op1.Value) * c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "string":
                                                    {

                                                        string val = op1.Value+ " " + op2.Value;
                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                    }
                                                    break;
                                                case "char":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "int";
                                                                    int c = char.Parse(op1.Value);
                                                                    double result = c * int.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "int";
                                                                    int c = char.Parse(op2.Value);
                                                                    int a = char.Parse(op1.Value);
                                                                    int result = a * c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "int";
                                                                    int c = char.Parse(op2.Value);
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    int result = c * boolean;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    int c = char.Parse(op2.Value);
                                                                    double result = c * double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "bool":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "int";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    int result = boolean * int.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "int";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    int c = char.Parse(op2.Value);
                                                                    int result = boolean * c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result = boolean * double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "bool";
                                                                    String boolean = "false";
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        if(op2.Value == "true")
                                                                        {
                                                                            boolean = "true";
                                                                        }
                                                                    }
                                                                    response.Value= boolean;
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "/":
                                        {
                                           var op2 = execute(hijos[2]);
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) / double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) / double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) / char.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "int";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    int result = int.Parse(op1.Value) / boolean;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) / int.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = double.Parse(op1.Value) / double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "double";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result = double.Parse(op1.Value) / boolean;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "double";
                                                                    int c = char.Parse(op2.Value);
                                                                    double result = double.Parse(op1.Value) / c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "string":
                                                    {

                                                        string val = op1.Value+ " " + op2.Value;
                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                    }
                                                    break;
                                                case "char":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "double";
                                                                    int c = char.Parse(op1.Value);
                                                                    double result = c / double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "double";
                                                                    double c = char.Parse(op2.Value);
                                                                    int a = char.Parse(op1.Value);
                                                                    double result = a / c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "int";
                                                                    int c = char.Parse(op1.Value);
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    int result = c / boolean;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    int c = char.Parse(op1.Value);
                                                                    double result = c / double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "bool":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "double";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result =  boolean / double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "double";
                                                                    int c = char.Parse(op2.Value);
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result = boolean / c;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    int boolean = 0;
                                                                    if (op1.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result = boolean / double.Parse(op2.Value);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "^":
                                        {
                                           var op2 = execute(hijos[2]);
                                            switch (op1.Type)
                                            {
                                                case "int":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "int";
                                                                    double result = Math.Pow(int.Parse(op1.Value), int.Parse(op2.Value));
                                                                    response.Value= String.Concat((int)result);
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = Math.Pow(double.Parse(op1.Value),double.Parse( op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "int";
                                                                    int c = char.Parse(op2.Value);
                                                                    double result = Math.Pow(int.Parse(op1.Value), c);
                                                                    response.Value= String.Concat((int)result);
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "int";
                                                                    int boolean;
                                                                    if (op2.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    else boolean = 0;
                                                                    double result = Math.Pow( int.Parse(op2.Value),boolean);
                                                                    response.Value= String.Concat((int) result);
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "double":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "int":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = Math.Pow(double.Parse(op1.Value), double.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "double":
                                                                {
                                                                    response.Type = "double";
                                                                    double result = Math.Pow(double.Parse(op1.Value), double.Parse(op2.Value));
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "bool":
                                                                {
                                                                    response.Type = "double";
                                                                    int boolean = 0;
                                                                    if (op2.Value == "true")
                                                                    {
                                                                        boolean = 1;
                                                                    }
                                                                    double result = Math.Pow(double.Parse(op1.Value), boolean);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            case "char":
                                                                {
                                                                    response.Type = "double";
                                                                    int c = char.Parse(op2.Value);
                                                                    double result = Math.Pow(double.Parse(op1.Value), c);
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "string":
                                                    {
                                                        string val = op1.Value+ " " + op2.Value;
                                                        Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                    }
                                                    break;
                                                case "char":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "char":
                                                                {
                                                                    response.Type = "int";    
                                                                    char c = char.Parse(op2.Value);
                                                                    char a = char.Parse(op1.Value);
                                                                    double result = Math.Pow(a, c);
                                                                    response.Value= String.Concat((int)result);
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "bool":
                                                    {
                                                        switch (op2.Type)
                                                        {
                                                            case "bool":
                                                                {
                                                                    response.Type = "bool";
                                                                    bool result = op1.Value == op2.Value;
                                                                    response.Value= String.Concat(result).ToLower();
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    string val = op1.Value+ " " + op2.Value;
                                                                    Errores.Add(new error(val, "Error semantico", "operacion invalida", response.Line, response.Column));
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                    break;
                default:
                    response.Type = node_.Term.Name;
                    foreach (var element in hijos)
                    {
                        execute(element);
                    }
                    break;
            }
            return response;
        }
    }
}
