using _OLC1__Proyecto2.Classes;
using Irony.Parsing;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace _OLC1__Proyecto2
{
    public partial class Form1 : Form
    {
        private string path;
        private ParseTreeNode root;
        public PictureBox actual = null;
        RichTextBox txtactual = null;
        ParseTree CurrentNode = null;Boolean BadEntrance = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void clean()
        {
            txtConsole.Text = "";
        }

        private void fillData(Dictionary<string,Var> variables)
        {
            DataTable data = new DataTable("variables");
            DataColumn c1 = new DataColumn("Nombre");
            DataColumn c0 = new DataColumn("Valor");
            DataColumn c2 = new DataColumn("Tipo");

            //Add the Created Columns to the Datatable
            data.Columns.Add(c0);
            data.Columns.Add(c1);
            data.Columns.Add(c2);
            foreach (KeyValuePair<string, Var> entry in variables)
            {
                Var aVariable = entry.Value;
                DataRow row = data.NewRow();
                row["Nombre"] = entry.Key;
                row["Valor"] = aVariable.Value;
                row["Tipo"] = aVariable.Type;
                data.Rows.Add(row);
            }
            tbVar.DataSource = data;
            tbVar.Columns["Nombre"].Width = 200;
            tbVar.Columns["Valor"].Width = 200;
            tbVar.Columns["Tipo"].Width = 200;
        }

        public void Analize()
        {
            BadEntrance = false;
            Update();
            Parser p = new Parser(new LanguageData(new Language()));
            ParseTree tree = p.Parse(obtenerentrada().Text);

            if (tree.Root != null) {
                
                Semantic semanticA = new Semantic();
                this.root = tree.Root;
                semanticA.execute(tree.Root);
                CurrentNode = tree;
                if(semanticA.Errores.Count == 0)
                {
                    if (semanticA.Variables.Count != 0)
                    {
                        Console.WriteLine("dentro");
                        fillData(semanticA.Variables);
                        makeMessages(semanticA.Shows);
                        
                    }
                    AddImages(semanticA.getListaLista());
                  

                    txtConsole.Text = semanticA.Console;
                }
                else
                {
                    
                  
                    Console.WriteLine("Errores semanticos");
                    foreach (var error in semanticA.Errores)
                    {
                        txtConsole.Text += error.Description + " " + error.Value + " " + error.Line + " " + error.Column + "\n";
                    }
                }
            }
            else
            {
                BadEntrance = true;
                CurrentNode = tree;
                showErrors(tree);
                Console.WriteLine("salida incorrecta");
            }
        }

        private void showErrors(ParseTree tree)
        {
            foreach(var error in tree.ParserMessages)
            {
                Console.WriteLine("se encontro un error: {0} en la linea: {1} columna: {2}", error.Message, error.Location.Line, error.Location.Column);

            }
        }

        private void makeMessages(List<string> shows)
        {
            foreach(var msg in shows)
            {
                var data = msg.Split('@');
                MessageBox.Show(data[1], data[0]);

            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            using (var reader = new StreamReader(myStream))
                            {
                                path = openFileDialog1.FileName;
                                txtInput.Text = reader.ReadToEnd();
                            }
                        }
                    }
                    clean();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (path != null)
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(txtInput.Text);
                    MessageBox.Show("Guardado exitosamente", "Guardar");
                }
            }
            else
            {
                saveAs();
            }

        }

        private void saveAs()
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(txtInput.Text);
                }
            }
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        private void compilarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Analize();
        }

        private void aSTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Plotter.makeGraphic(this.root);
        }

        private void AddImages(List<List<Figuras>> lista)
        {
            foreach (var item in lista)
            {
                GenerarImagenes(item);
                
            }
        }

        private void GenerarImagenes(List<Figuras> milista)
        {
            
            Form ventana = new Form();
            ventana.WindowState = FormWindowState.Maximized;
            PictureBox lienzo = new PictureBox();
            lienzo.Width = Screen.PrimaryScreen.Bounds.Width;
            lienzo.Height = Screen.PrimaryScreen.Bounds.Width;
            setPictureBox(lienzo);
            ventana.Controls.Add(lienzo);

            ventana.Show();
            ventana.Update();
            BufferDeImagenes(milista);

        }

        public PictureBox GetPictureBox()
        {
            return actual;
        }

        public void setPictureBox(PictureBox selectedP)
        {
            actual = selectedP;
        }

        public void rectangulolleno(String color, int pt1, int pt2, int pt3, int pt4)

        {
            System.Drawing.Color mycolor = System.Drawing.ColorTranslator.FromHtml(color);
            System.Drawing.SolidBrush my = new System.Drawing.SolidBrush(mycolor);
            System.Drawing.Graphics form;
            form = GetPictureBox().CreateGraphics();
            form.FillRectangle(my, new Rectangle(pt1, pt2, pt3, pt4));
            my.Dispose();
            form.Dispose();


        }

        public void circulorelleno(String color, int x, int y, int diametro1, int diametro2)
        {
            System.Drawing.Color mycolor = System.Drawing.ColorTranslator.FromHtml(color);
            System.Drawing.SolidBrush my = new System.Drawing.SolidBrush(mycolor);
            System.Drawing.Graphics form;
            form = GetPictureBox().CreateGraphics();
            form.FillEllipse(my, new RectangleF(x, y, diametro1, diametro2));

            my.Dispose();
            form.Dispose();
        }

        public void triangulorelleno(String color, int p1, int p1_1, int p2, int p2_2, int p3, int p3_2)
        {
            System.Drawing.Color mycolor = System.Drawing.ColorTranslator.FromHtml(color);
            Point[] pnt = new Point[3];
            pnt[0].X = p1;
            pnt[0].Y = p1_1;

            pnt[1].X = p2;
            pnt[1].Y = p2_2;

            pnt[2].X = p3;
            pnt[2].Y = p3_2;
            System.Drawing.SolidBrush my = new System.Drawing.SolidBrush(mycolor);
            System.Drawing.Graphics form;
            form = GetPictureBox().CreateGraphics();
            form.FillPolygon(my, pnt);

            my.Dispose();
            form.Dispose();
        }

        public void linea(String color, int x, int y, int width, int height, int thickness)
        {
            System.Drawing.Color mycolor = System.Drawing.ColorTranslator.FromHtml(color);
            Graphics papel;
            papel = GetPictureBox().CreateGraphics();
            Pen lapiz = new Pen(mycolor);
            lapiz.Width = thickness;
            papel.DrawLine(lapiz, x, y, width, height);

        }

        public void triangulosinrelleno(String color, int p1, int p1_1, int p2, int p2_2, int p3, int p3_2, int thick)
        {
            System.Drawing.Color mycolor = System.Drawing.ColorTranslator.FromHtml(color);
            Point[] pnt = new Point[3];
            pnt[0].X = p1;
            pnt[0].Y = p1_1;

            pnt[1].X = p2;
            pnt[1].Y = p2_2;

            pnt[2].X = p3;
            pnt[2].Y = p3_2;

            Graphics papel;
            papel = GetPictureBox().CreateGraphics();
            Pen lapiz = new Pen(mycolor);
            lapiz.Width = thick;
            papel.DrawPolygon(lapiz, pnt);

        }

        public void circulosinrelleno(String color, int p1, int p2, int p3, int p4, int thick)
        {
            System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml(color);
            Graphics papel;
            papel = GetPictureBox().CreateGraphics();
            Pen lapiz = new Pen(col);
            lapiz.Width = thick;
            papel.DrawEllipse(lapiz, p1, p2, p3, p4);
        }

        public void rectangulosinrelleno(String color, int p1, int p2, int p3, int p4, int thick)
        {
            System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml(color);
            Graphics papel;
            papel = GetPictureBox().CreateGraphics();
            Pen lapiz = new Pen(col);
            lapiz.Width = 3;
            papel.DrawRectangle(lapiz, p1, p2, p3, p4);
        }

        private void BufferDeImagenes(List<Figuras> ListaFiguras)
        {
            List<Figuras> asdf = ListaFiguras; // NO CREAR NUEVA SINO LLAMAR DE LA CLASE DONDE ESTE

            foreach (var item in asdf)
            {


                Console.WriteLine("Figuraas dentro");
                String nombre = item.Tipofigura;// obtener de la pila
                switch (nombre)
                {
                    case "triangle":
                        string solido = item.Solid1; // obtener la propiead solid de la tabla
                        switch (solido)
                        {
                            case "true":
                                // primer valor es el color

                                triangulorelleno(item.Color, item.Pt1, item.Pt2, item.Pt3, item.Pt4, item.Pt5, item.Pt6); // mandar los valores y convertirlos a int
                                break;

                            case "false":
                                // primer valor es el color
                                triangulosinrelleno(item.Color, item.Pt1, item.Pt2, item.Pt3, item.Pt4, item.Pt5, item.Pt6, item.Thickness); // mandar valores
                                                                                                                                             // el ultimo valor en sin relleno es el grosor
                                break;
                        }
                        break;
                    case "square":
                        string solido2 = item.Solid1; // obtener la propiead solid de la tabla
                        switch (solido2)
                        {
                            case "true":
                                // primer valor es el color
                                rectangulolleno(item.Color, item.Pt1, item.Pt2, item.Pt3, item.Pt4);
                                break;
                            case "false":
                                // primer valor es el color
                                rectangulosinrelleno(item.Color, item.Pt1, item.Pt2, item.Pt3, item.Pt4, item.Thickness); // ultimo valor es el grosor
                                break;
                        }
                        break;
                    case "circle":
                        Console.WriteLine("dentro circuli");
                        string solido3 = item.Solid1; // obtener la propiead solid de la tabla
                        switch (solido3)
                        {
                            case "true":
                                // primer valor es el color
                                
                                circulorelleno(item.Color, item.Pt1, item.Pt2, item.Pt3, item.Pt4);
                                break;
                            case "false":
                                // primer valor es el color
                                Console.WriteLine(item.Color + " -" + item.Pt1 + "- " + item.Pt3 + "- " + item.Pt4);
                                circulosinrelleno(item.Color, item.Pt1, item.Pt2, item.Pt3, item.Pt4, item.Thickness); // ultimo valor es el grososor
                                break;
                        }
                        break;
                    case "line":
                        linea(item.Color, item.Pt1, item.Pt2, item.Pt3, item.Pt4, item.Thickness); // ultimo valor es el grosor
                                                                                                   // primer valor es el color

                        break;

                }


            }
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void crearToolStripMenuItem_Click(object sender, EventArgs e)
        {

            String ingreso = "";
            Object InputDialog = Interaction.InputBox("New Tab Name", "", ingreso, -1, -1);
            String TabName = InputDialog.ToString();
            TabPage newpage = new TabPage(TabName); RichTextBox newtxt = new RichTextBox();

            newtxt.Width = 685;
            newtxt.Height = 280;
            newtxt.BorderStyle = BorderStyle.None;
            newtxt.Font = new Font("Consolas", 10);

            newpage.Controls.Add(newtxt);
            tabControl1.TabPages.Add(newpage);
        }

        public void Update()
        {

            var tab = tabControl1.SelectedTab;
            var controles = tab.Controls;
            foreach (var item in controles)
            {
                if (item is RichTextBox)
                {
                    setear(((RichTextBox)item));
                }
            }
        }

        public RichTextBox obtenerentrada()
        {
            return txtactual;
        }

        public void setear(RichTextBox txt)
        {
            txtactual = txt;
        }

        private void erroresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BadEntrance == true)
            {
                HTMLERRO();
            }
        }
        public void HTMLERRO()
        {
            StreamWriter write = new StreamWriter("Mistake.html");
            write.Write(BODYHTML());
            write.Close();
            Process.Start("Mistake.html");
        }
        
        public String BODYHTML()
        {
            String info = " <html> \n" +
      "<head><title>Mistake</title></head> \n" +
      "<style type=\"text/css\">" +
      Style()+
            "</style > " +
      "<body>" +
      "<h1>Tabla de Informacion de Errores</h1>" + "<div class=\"container\">"+
      "<table class=\"gridtable\">" +
                               "<tr>" +
      "<th>Descripcion</th>" +
      "<th>Tipo</th>" +
      "<th>Fila</th>" +
      "<th>Columna</th>" +

      "</tr>" + Body(CurrentNode) + "</table>" + "</div>"+
      "</body>" +
      "</html>";
            return info;

        }

        public String Style()
        {
            string sb = "html," +
 "body {" +
 "	height: 100%;" +
 "}" +
 "body {" +
 "	margin: 0;" +
 "	background: linear-gradient(45deg, #49a09d, #5f2c82);" +
 "	font-family: sans-serif;" +
 "	font-weight: 100;" +
 "}" +
 ".container {" +
 "	position: absolute;" +
 "	top: 50%;" +
 "	left: 50%;" +
 "	transform: translate(-50%, -50%);" +
 "}" +
 "table {" +
 "	width: 800px;" +
 "	border-collapse: collapse;" +
 "	overflow: hidden;" +
 "	box-shadow: 0 0 20px rgba(0,0,0,0.1);" +
 "}" +
 "th," +
 "td {" +
 "	padding: 15px;" +
 "	background-color: rgba(255,255,255,0.2);" +
 "	color: #fff;" +
 "}" +
 "th {" +
 "	text-align: left;" +
 "}" +
 "thead {" +
 "	th {" +
 "		background-color: #55608f;" +
 "	}" +
 "}" +
 "tbody {" +
 "	tr {" +
 "		&:hover {" +
 "			background-color: rgba(255,255,255,0.3);" +
 "		}" +
 "	}" +
 "	td {" +
 "		position: relative;" +
 "		&:hover {" +
 "			&:before {" +
 "				content: \"\";" +
 "				position: absolute;" +
 "				left: 0;" +
 "				right: 0;" +
 "				top: -9999px;" +
 "				bottom: -9999px;" +
 "				background-color: rgba(255,255,255,0.2);" +
 "				z-index: -1;" +
 "			}" +
 "		}" +
 "	}" +
 "}";
            return sb;
        }
        public String Body(ParseTree root)
        {
            String cuerpo = "";
            for (int i = 0; i < root.ParserMessages.Count; i++)
            {
                cuerpo += "<tr>" + "\n" +
                "<td>" + root.ParserMessages.ElementAt(i).Level.ToString() + "</td>" +
                "<td>" + root.ParserMessages.ElementAt(i).Message + "</td>" +
                "<td>" + root.ParserMessages.ElementAt(i).Location.Line + "</td>" +
                "<td>" + root.ParserMessages.ElementAt(i).Location.Column + "</td>" + "</tr>" + "\n";
            }
            return cuerpo;
        }

        // TABLA DE TOKENS-----------------------------------
        public void obtenerhtmltokens()
        {
            StreamWriter write = new StreamWriter("Tokens.html");
            write.Write(cuerpotokens());
            write.Close();
            Process.Start("Tokens.html");
        }

        public String cuerpotokens()
        {
            String info = " <html> \n" +
      "<head><title>Tabla Tokens</title></head> \n" +
      "<style style=\"text/css\"> \n" +
             Style()+
                "</style>" +
      "<body>" +
      "<h1>Tabla Tokens</h1>" +
      "<table>" +
                               "<tr>" +
      "<th>Tipo</th>" +
      "<th>Lexema</th>" +
      "<th>Fila</th>" +
      "<th>Columna</th>" +

      "</tr>" + internalcode(CurrentNode) + "</table>" +
      "</body>" +
      "</html>";
            return info;

        }

        public String internalcode(ParseTree root)
        {
            String body = "";
            foreach (Token tk in root.Tokens)
            {
                if (tk == root.Tokens.Last())
                {

                }
                else
                {
                    String[] temp = (tk.ToString()).Split(new char[0], 2);
                    body += "<tr>" + "\n" +
                        "<td>" + temp[1] + "</td>" +
                        "<td>" + tk.Value.ToString() + "</td>" +
                        "<td>" + tk.Location.Line + "</td>" +
                        "<td>" + tk.Location.Column + "</td>" +
                        "</tr>" + "\n";
                }
            }
            return body;


        }

        private void tokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            obtenerhtmltokens();
        }
    }
}
