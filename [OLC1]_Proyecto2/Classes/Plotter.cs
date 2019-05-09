using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _OLC1__Proyecto2.Classes
{
    class Plotter
    {
        private static String graph = "";
        public static void makeGraphic(ParseTreeNode raiz)
        {
            using (StreamWriter f = new StreamWriter("AST.txt"))
            {
                f.Write("digraph lista{ rankdir=TB;node[shape = box, style = filled, color = white]; ");
                graph = "";
                Generar(raiz);
                f.Write(graph);
                f.Write("}");
                f.Close();
            }
            makeImage();
        }

        private static void Generar(ParseTreeNode raiz)
        {
            graph = graph + "nodo" + raiz.GetHashCode() + "[label=\"" + raiz.ToString().Replace("\"", "\\\"") + " \", fillcolor=\"LightBlue\", style =\"filled\", shape=\"box\"]; \n";
            if (raiz.ChildNodes.Count > 0)
            {
                ParseTreeNode[] hijos = raiz.ChildNodes.ToArray();
                for (int i = 0; i < raiz.ChildNodes.Count; i++)
                {
                    Generar(hijos[i]);
                    graph = graph + "nodo" + raiz.GetHashCode() + " -> nodo" + hijos[i].GetHashCode() + "\n";
                }
            }
        }
        private static void makeImage()
        {
            try
            {
                var command = string.Format("dot -Tpng AST.txt -o AST.png");
                var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + command);
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception x)
            {

            }
        }
    }
}
