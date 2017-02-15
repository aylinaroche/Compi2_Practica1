using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;

namespace SBScript
{
    public partial class Form1 : Form
    {
        String ruta = "";
        ArrayList listaRuta = new ArrayList();
        int num = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAgregarPestania_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Add(nombrePestania());
            foreach (TabPage tab in tabControl1.TabPages)
            {
                RichTextBox rich = new RichTextBox();
                rich.Name = "Tab";
                rich.Width = 450;
                rich.Height = 350;

                String ruta = "";
                tab.Controls.Add(rich);
                //  tab.Controls.Add(ruta);
            }
        }

        public String nombrePestania()
        {
            num += 1;
            String nombre = "Doc" + num;
            return nombre;
        }
        private void btnElminarPestania_Click(object sender, EventArgs e)
        {

            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\Users\\Aylin\\Documents";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt| SBS files (*.*)|*.sbs";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                // MessageBox.Show(openFileDialog1.FileName);
                ruta = openFileDialog1.FileName;
                var rich = (RichTextBox)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
                rich.Text = sr.ReadToEnd();
                sr.Close();
            }

            txtConsola.Text += ">> Archivo abierto correctamente.\n";
            Pestania p = new Pestania();
            p.nombre = tabControl1.SelectedTab.ToString();
            p.ruta = ruta;
            listaRuta.Add(p);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            String rutaAux = "";
            for (int i = 0; i < listaRuta.Count; i++)
            {
                Pestania p = (Pestania)listaRuta[i];
                if (p.nombre == tabControl1.SelectedTab.ToString())
                {
                    rutaAux = p.ruta;
                }
            }

            if (rutaAux != "")
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(ruta);
                try
                {
                    fi.Delete();
                }
                catch (System.IO.IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                System.IO.StreamWriter streamWriter = new StreamWriter(ruta, true);
                var rich = (RichTextBox)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
                streamWriter.WriteLine(rich.Text);
                streamWriter.Close();
                txtConsola.Text += ">> Archivo guardado correctamente.\n";
            }
            else
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.DefaultExt = "*.sbs";
                saveFileDialog1.Filter = "sbs Files|*.sbs";
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFileDialog1.FileName.Length > 0)
                {
                    var rich = (RichTextBox)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
                    rich.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                }
                txtConsola.Text += ">> Archivo guardado correctamente.\n";
            }
        }

        private void btnGuardarComo_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.DefaultExt = "*.sbs";
            saveFileDialog1.Filter = "sbs Files|*.sbs";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFileDialog1.FileName.Length > 0)
            {
                var rich = (RichTextBox)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
                rich.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }
            txtConsola.Text += ">> Guardar archivo como.\n";
        }

        class Pestania
        {
            public String nombre;
            public String ruta;
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            Limpiar();
            var rich = (RichTextBox)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
            ParseTreeNode resultado = Analizador.analizar(rich.Text);

            if (resultado != null)
            {
                MessageBox.Show("El arbol fue construido correctamente");
                PrimerRecorrido.action(resultado);
                ParseTreeNode nodoPrincipal = Metodo_Funcion.buscarMetodo("MAIN");
                if (nodoPrincipal != null)
                {
                    Variables.pilaAmbito.Push("Principal");
                    Variables.nivelAmbito += 1;
                    SegundoRecorrido.action(nodoPrincipal);

                }
                imprimirVariables();
                imprimir();

            }
            else
            {
                MessageBox.Show("ERROR: Deberia de revisar la cadena de entrada");
            }

        }

        public void imprimir()
        {
            for (int i = 0; i < Listas.MensajeConsola.Count; i++)
            {
                String v = (String)Listas.MensajeConsola[i];
                txtConsola.Text += v;
            }
        }

        public void imprimirVariables()
        {
            for (int i = 0; i < Variables.listaVariables.Count; i++)
            {
                Variable v = (Variable)Variables.listaVariables[i];
                txtConsola.Text += " -> "+v.tipo+", "+ v.nombre+", "+v.valor+", "+v.ambito+"\n";
            }
            txtConsola.Text += "\n ********************* \n\n";
            for (int i = 0; i < Metodo_Funcion.listaMetodoFuncion.Count; i++)
            {
                MF m = (MF)Metodo_Funcion.listaMetodoFuncion[i];
                txtConsola.Text += " -> " + m.tipo + ", " + m.nombre +"\n";
                for (int j = 0; j < m.parametro.Count; j++)
                {
                    Parametro p = (Parametro)m.parametro[j];
                    txtConsola.Text += "        -" + p.tipo + ", " + p.nombre + "\n";

                }
            }
            txtConsola.Text += "\n ********************* \n\n\n";
        }

        public void Limpiar()
        {
            Listas.MensajeConsola.Clear();
            Variables.listaVariables.Clear();
            Metodo_Funcion.listaMetodoFuncion.Clear();
            Reporte.errores.Clear();
            txtConsola.Clear();
        }
        private void btnReporte_Click(object sender, EventArgs e)
        {
            Reporte.generarReporte();
        }

        private void btnAlbum_Click(object sender, EventArgs e)
        {

        }
    }
}
