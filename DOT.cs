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
    class DOT
    {  
        private static int contador;
        private static String grafo;
        public static String getDOT(ParseTreeNode raiz)
        {
            grafo = "digraph G{";
            grafo += "nodo0[label= \"" + escapar(raiz.ToString()) + "\"];\n";
            contador = 1;
            recorrerAST("nodo0", raiz);
            grafo += "}";

            return grafo;
        }
        private static void recorrerAST(String padre, ParseTreeNode hijos)
        {
            foreach (ParseTreeNode hijo in hijos.ChildNodes)
            {
                String nombreHijo = "nodo" + contador.ToString();
                grafo += nombreHijo + "[label= \"" + escapar(hijo.ToString()) + "\"]";
                grafo += padre + "->" + nombreHijo + ";\n";
                contador++;
                recorrerAST(nombreHijo, hijo);
            }
        }
        private static String escapar(String cadena)
        {
            cadena = cadena.Replace("\\", "\\\\");
            cadena = cadena.Replace("\"", "\\\\");
            return cadena;
        }
    }

    class DIBUJAR
    {
        private static int contador;
        private static String grafo;
        public static String getDotFuncion(ParseTreeNode raiz)
        {
            grafo = "digraph G{";
            grafo += "nodo0[label= \"" + escapar(raiz.ToString()) + "\"];\n";
            contador = 1;
            recorrerAstFuncion("nodo0", raiz);
            grafo += "}";

            return grafo;
        }
        private static void recorrerAstFuncion(String padre, ParseTreeNode hijos)
        {
            foreach (ParseTreeNode hijo in hijos.ChildNodes)
            {
                String nombreHijo = "nodo" + contador.ToString();
                String nombre = escapar(hijo.ToString());
                if (nombre == "SI")
                {
                    grafo += nombreHijo + "[label= \"" + nombre + "\"]";
                    grafo += padre + "->" + nombreHijo + ";\n";
                    contador++;
                }
                else if (nombre == "OP")
                {
                    nombre = "EXPRESION";
                    grafo += nombreHijo + "[label= \"" + nombre + "\"]";
                    grafo += padre + "->" + nombreHijo + ";\n";
                    contador++;
                }
                else if (nombre == "INSTRUCCIONES")
                {
                    nombre = "INSTRUCCIONES";
                    grafo += nombreHijo + "[label= \"" + nombre + "\"]";
                    grafo += padre + "->" + nombreHijo + ";\n";
                    contador++;
                }
                else if (nombre == "VAR")
                {
                    nombre = "LISTA";
                    grafo += nombreHijo + "[label= \"" + nombre + "\"]";
                    grafo += padre + "->" + nombreHijo + ";\n";
                    contador++;
                }
                else if (nombre == "DECLARACION")
                {
                    nombre = "DECLARACION";
                    grafo += nombreHijo + "[label= \"" + nombre + "\"]";
                    grafo += padre + "->" + nombreHijo + ";\n";
                    contador++;
                }
                else if (nombre == "RETORNO")
                {
                    nombre = "RETORNO";
                    grafo += nombreHijo + "[label= \"" + nombre + "\"]";
                    grafo += padre + "->" + nombreHijo + ";\n";
                    contador++;
                }
                else
                
                    recorrerAstFuncion(nombreHijo, hijo);
                
            }
        }
        private static String escapar(String cadena)
        {
            cadena = cadena.Replace("\\", "\\\\");
            cadena = cadena.Replace("\"", "\\\\");
            return cadena;
        }
    }
}
