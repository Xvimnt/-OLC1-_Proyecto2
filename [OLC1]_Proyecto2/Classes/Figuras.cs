using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1__Proyecto2.Classes
{
    class Figuras
    {
        String tipofigura = "";
        String color = "";
        int pt1 = 0;
        int pt2 = 0;
        int pt3 = 0;
        int pt4 = 0;
        int pt5 = 0;
        int pt6 = 0;
        int thickness = 0;
        String Solid = "";
        public Figuras(String tipo, String color, int p1, int p2, int p3, int p4, int p5, int p6, int grosor, String s)
        {
            this.Tipofigura = tipo;
            this.Color = color;
            this.Pt1 = p1;
            this.Pt2 = p2;
            this.Pt3 = p3;
            this.Pt4 = p4;
            this.Pt5 = p5;
            this.Pt6 = p6;
            this.Thickness = grosor;
            this.Solid1 = s;

        }

        public string Tipofigura { get => tipofigura; set => tipofigura = value; }
        public string Color { get => color; set => color = value; }

        public string Solid1 { get => Solid; set => Solid = value; }
        public int Pt1 { get => pt1; set => pt1 = value; }
        public int Pt2 { get => pt2; set => pt2 = value; }
        public int Pt3 { get => pt3; set => pt3 = value; }
        public int Pt4 { get => pt4; set => pt4 = value; }
        public int Pt5 { get => pt5; set => pt5 = value; }
        public int Pt6 { get => pt6; set => pt6 = value; }
        public int Thickness { get => thickness; set => thickness = value;
        }
    }
}
