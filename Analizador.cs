using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace SBScript
{
    class Analizador
    {
        public static ParseTreeNode analizar(String cadena)
        {
            Gramatica gramatica = new Gramatica();

            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            //  MessageBox.Show("parser"); //Error de Gramatica
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;
            //foreach (var item in  arbol.ParserMessages)
            //{
            //   // Irony.LogMessage log = arbol.ParserMessages[item];
            //    //log.Location.Line.ToString();
            //}
            for (int i = 0; i < arbol.ParserMessages.Count; i++)
            {
                Irony.LogMessage log = arbol.ParserMessages[i];
                String[] dato = log.Level.ToString().Split(':');
                String tipo = "";
                String mensaje = log.Message.ToString();
                if (dato[0] == "Invalid character")
                {
                    tipo = "Error Lexico";
                    mensaje = mensaje.Replace("Invalid character", "Caracter invalido:");
                }
                else
                {
                    tipo = "Error Sintactico";
                    mensaje = mensaje.Replace("Syntax error, expected:", "Se esperaba:");
                }
                Reporte.agregarError(mensaje, tipo, log.Location.Line, log.Location.Column);
            }
            if (raiz == null)
            {
                return raiz;
            }
            else
            {
                generarArbol(raiz);
                return raiz;
            }
        }

        private static void generarArbol(ParseTreeNode raiz)
        {
            String grafodot = DOT.getDOT(raiz);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("C:/Users/Aylin/Documents/Visual Studio 2015/Projects/SBScript/arbol.dot"))
            {
                file.WriteLine(grafodot);
            }
            ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Program Files (x86)\\Graphviz 2.28\\bin\\dot.exe");
            startInfo.Arguments = "dot -Tpng \"C:/Users/Aylin/Documents/Visual Studio 2015/Projects/SBScript/arbol.dot\" -o \"C:/Users/Aylin/Documents/Visual Studio 2015/Projects/SBScript/arbol.png\"";
            try
            {
                Process.Start(startInfo);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }


        }

        public static void generarAST(ParseTreeNode raiz, String nombre)
        {
            if(raiz== null)
            {
                Reporte.agregarMensajeError("La funcion es vacia", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                return;
            }

            String grafodot = DIBUJAR.getDotFuncion(raiz);
            String ruta = "";
            if (Listas.ruta == "")
            {
                ruta = Listas.rutaDefinida + nombre;
            }
            else
            {
                if (Directory.Exists(Listas.ruta))
                {
                    ruta = Listas.quitarEspaciosFinal(Listas.ruta) + "\\" + nombre;
                }
                else
                {
                    Reporte.agregarError("No existe ruta: " + ruta, "Error General", 0, 0);
                    ruta = Listas.rutaDefinida + nombre;
                }

            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(ruta + ".dot"))
            {
                file.WriteLine(grafodot);
            }
            ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Program Files (x86)\\Graphviz 2.28\\bin\\dot.exe");
            startInfo.Arguments = "dot -Tpng \"" + ruta + ".dot\" -o \"" + ruta + ".png\"";
            try
            {
                Process.Start(startInfo);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        public static ParseTreeNode analizarEXP(String cadena)
        {
            GramaticaEXP gramatica = new GramaticaEXP();

            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;
            if (raiz == null)
            {
                return raiz;
            }
            else
            {
                //    generarArbolEXP(raiz);
                return raiz;
            }
        }

        public static void generarEXP(ParseTreeNode raiz, String nombre)
        {
            if (raiz == null)
            {
                Reporte.agregarMensajeError("La expresion es vacia", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                return;
            }
            String grafodot = DIBUJAR.getDotEXP(raiz);
            String ruta = "";
            if (Listas.ruta == "")
            {
                ruta = Listas.rutaDefinida + nombre;
            }
            else
            {
                if (Directory.Exists(Listas.ruta))
                {
                    ruta = Listas.quitarEspaciosFinal(Listas.ruta) + "\\" + nombre;
                }
                else
                {
                    Reporte.agregarError("No existe ruta: " + ruta, "Error General", 0, 0);
                    ruta = Listas.rutaDefinida + nombre;
                }
               
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(ruta + ".dot"))
            {
                file.WriteLine(grafodot);
            }
            ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Program Files (x86)\\Graphviz 2.28\\bin\\dot.exe");
            startInfo.Arguments = "dot -Tpng \"" + ruta + ".dot\" -o \"" + ruta + ".png\"";
            try
            {
                Process.Start(startInfo);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

    }
}

