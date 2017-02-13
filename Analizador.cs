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
           
            for(int i = 0; i< arbol.ParserMessages.Count; i++)
            {
                Irony.LogMessage log = arbol.ParserMessages[i];
                Reporte.agregarError(log.Message.ToString(), log.Level.ToString(), log.Location.Line, log.Location.Column);                
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
            }catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }


        }
    }
}

