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
            //MessageBox.Show("arbol");
            ParseTreeNode raiz = arbol.Root;
            //MessageBox.Show("termino");
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
            }catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }


        }
    }
}

