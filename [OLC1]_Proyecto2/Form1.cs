using _OLC1__Proyecto2.Classes;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace _OLC1__Proyecto2
{
    public partial class Form1 : Form
    {
        private string path;
        private ParseTreeNode root;
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
            Parser p = new Parser(new LanguageData(new Language()));
            ParseTree tree = p.Parse(txtInput.Text);

            if (tree.Root != null) {
                
                Semantic semanticA = new Semantic();
                this.root = tree.Root;
                semanticA.execute(tree.Root);
                if(semanticA.Errores.Count == 0)
                {
                    if (semanticA.Variables.Count != 0)
                    {
                        fillData(semanticA.Variables);
                        makeMessages(semanticA.Shows);
                    }
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
    }
}
