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
        public static ArrayList listaRuta = new ArrayList();
        public static String nombreTab;
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
            String texto = "";
            String nombre = "";
            String carpeta = "";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                String[] dato = openFileDialog1.FileName.Split('\\');
                nombre = dato[dato.Length - 1];
                ruta = openFileDialog1.FileName;
                for (int i = 0; i < dato.Length - 1; i++)
                {
                    carpeta += dato[i] + "\\";
                }
                texto = sr.ReadToEnd();
                sr.Close();
            }

            txtConsola.Text += ">> Archivo abierto correctamente.\n";
            Pestania p = new Pestania();
            p.nombre = nombre;
            p.ruta = ruta;
            p.carpeta = carpeta;
            listaRuta.Add(p);
            tabControl1.TabPages.Add(nombre);
            foreach (TabPage tab in tabControl1.TabPages)
            {
                RichTextBox rich = new RichTextBox();
                rich.Name = "Tab";
                rich.Width = 450;
                rich.Height = 350;
                tab.Controls.Add(rich);
                rich.Text = texto;
            }
            var richt = (RichTextBox)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];

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

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            Limpiar();
            Listas.archivo.Clear();
            String nombre = tabControl1.SelectedTab.AccessibilityObject.Name.ToString();
            Listas.archivo.Push(nombre);
            nombreTab = nombre;
            var rich = (RichTextBox)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
            ParseTreeNode resultado = Analizador.analizar(rich.Text);

            if (resultado != null)
            {
                MessageBox.Show("El arbol fue construido correctamente");
                PrimerRecorrido.action(resultado);
                RecorridoGlobal.action(resultado);
                ParseTreeNode nodoPrincipal = Metodo_Funcion.buscarMetodo("MAIN");
                if (nodoPrincipal != null)
                {
                    Variables.pilaAmbito.Push("Principal");
                    Variables.nivelAmbito += 1;
                    SegundoRecorrido.continuar = false;
                    SegundoRecorrido.detener = false;
                    SegundoRecorrido.retornar = false;
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
                txtConsola.Text += " -> " + v.tipo + ", " + v.nombre + ", " + v.valor + ", " + v.ambito + "\n";
            }
            txtConsola.Text += "********************* \n";
            for (int i = 0; i < Metodo_Funcion.listaMetodoFuncion.Count; i++)
            {
                MF m = (MF)Metodo_Funcion.listaMetodoFuncion[i];
                txtConsola.Text += " -> " + m.tipo + ", " + m.nombre + "\n";
                for (int j = 0; j < m.parametro.Count; j++)
                {
                    Parametro p = (Parametro)m.parametro[j];
                    txtConsola.Text += "        -" + p.tipo + ", " + p.nombre + "\n";

                }
            }
            txtConsola.Text += "\n ********************* \n\n";
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
            if (Listas.ruta == "")
            {
                System.Diagnostics.Process.Start(@"C:\\Users\\Aylin\\Documents\\Visual Studio 2015\\Projects\\SBScript\\Reportes");
            }
            else
            {
                System.Diagnostics.Process.Start(Listas.ruta);
            }
        }

        public void incluirArchivo(String archivo)
        {
            int i = 0;
            String carpeta = "";
            if (listaRuta.Count == 0)
            {
                Reporte.agregarMensajeError("No se ha podido incluir el archivo ya que no esta guardado", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                return;
            }

            for (i = 0; i < listaRuta.Count; i++) //Si la ruta esta vacia
            {
                Pestania p = (Pestania)listaRuta[i];
                if (p.nombre == nombreTab)
                {
                    carpeta = p.carpeta;
                }
            }

            if (carpeta == "")
            {
                Reporte.agregarMensajeError("No se ha podido incluir el archivo ya que no esta guardado", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                return;
            }
            else
            {
                String ruta = carpeta + archivo;
                String texto = "";
                try
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(ruta);
                    ruta = openFileDialog1.FileName;
                    texto = sr.ReadToEnd();
                    sr.Close();

                    Pestania p = new Pestania();
                    p.nombre = archivo;
                    p.ruta = ruta;
                    p.carpeta = carpeta;
                    listaRuta.Add(p);

                    MessageBox.Show(archivo);
                    txtConsola.Text += "\n* * * * * * * * " + archivo + " * * * * * * * ";
                    ParseTreeNode resultado = Analizador.analizar(texto);

                    if (resultado != null)
                    {
                        Listas.archivo.Push(archivo);
                        MessageBox.Show("El arbol fue construido correctamente");
                        PrimerRecorrido.action(resultado);
                        imprimirVariables();
                        imprimir();
                        Listas.archivo.Pop();
                    }
                    else
                    {
                        MessageBox.Show("ERROR: Deberia de revisar la cadena de entrada");
                    }


                }
                catch (Exception e)
                {
                    MessageBox.Show("Error;");
                }
            }
        }

        class Pestania
        {
            public String nombre;
            public String ruta;
            public String carpeta;
        }
    }

}
