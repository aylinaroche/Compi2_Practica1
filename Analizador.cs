using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Diagnostics;
using System.Windows.Forms;

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
                    mensaje = mensaje.Replace("Invalid character","Caracter invalido:");
                }
                else
                {
                    tipo = "Error Sintactico";
                    mensaje = mensaje.Replace("Syntax error, expected:", "Se esperaba:");
                }
                Reporte.agregarError(mensaje,tipo, log.Location.Line, log.Location.Column);
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
            String grafodot = DIBUJAR.getDotFuncion(raiz);
            String ruta = "C:/Users/Aylin/Documents/Visual Studio 2015/Projects/SBScript/Imagenes/" + nombre + ".dot";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(ruta))
            {
                file.WriteLine(grafodot);
            }
            ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Program Files (x86)\\Graphviz 2.28\\bin\\dot.exe");
            startInfo.Arguments = "dot -Tpng \"" + ruta + "\" -o \"C:/Users/Aylin/Documents/Visual Studio 2015/Projects/SBScript/Imagenes/" + nombre + ".png\"";
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
                generarArbolEXP(raiz);
                return raiz;
            }
        }

        private static void generarArbolEXP(ParseTreeNode raiz)
        {
            String grafodot = DIBUJAR.getDotEXP(raiz);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("C:/Users/Aylin/Documents/Visual Studio 2015/Projects/SBScript/arbolEXP.dot"))
            {
                file.WriteLine(grafodot);
            }
            ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Program Files (x86)\\Graphviz 2.28\\bin\\dot.exe");
            startInfo.Arguments = "dot -Tpng \"C:/Users/Aylin/Documents/Visual Studio 2015/Projects/SBScript/arbolEXP.dot\" -o \"C:/Users/Aylin/Documents/Visual Studio 2015/Projects/SBScript/arbolEXP.png\"";
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

