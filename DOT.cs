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
            grafo = "digraph G{ \n rankdir=LR;";
            grafo += "nodo0[shape=record, color=blue, label= \"" + escaparAST(raiz.ToString()) + "\"];\n";
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
                String nombre = escaparAST(hijo.ToString());

                grafo += nombreHijo + "[shape=record, color=blue, label= \"" + nombre + "\"]";
                grafo += padre + "->" + nombreHijo + ";\n";
                contador++;
                recorrerAstFuncion(nombreHijo, hijo);

            }
        }
        private static String escaparAST(String cadena)
        {
            cadena = cadena.Replace("\\", "\\\\");
            cadena = cadena.Replace("\"", "\\\\");
            return cadena;
        }

        ////**************************************

        public static String getDotEXP(ParseTreeNode raiz)
        {
            grafo = "digraph G{ \n";
            grafo += "nodo0[ color=purple, label= \"" + escapar(raiz.ToString()) + "\"];\n";
            contador = 1;
            recorrerEXP("nodo0", raiz);
            grafo += "}";

            return grafo;
        }
        private static void recorrerEXP(String padre, ParseTreeNode hijos)
        {
            if (hijos.ChildNodes.Count == 3)
            {
                String nombrehijo = "nodo" + contador.ToString();
                if (hijos.ChildNodes[1].Term.Name.ToString() == "E")
                {
                    recorrerEXP(nombrehijo, hijos.ChildNodes.ElementAt(1));
                }
                else
                {                
                    if (hijos.ChildNodes.ElementAt(1).Token.Text == "&&" || hijos.ChildNodes.ElementAt(1).Token.Text == "||" || hijos.ChildNodes.ElementAt(1).Token.Text == "|&")
                    {
                        grafo = grafo + nombrehijo + "[ color=purple, label=\"" + escapar(hijos.ChildNodes.ElementAt(1).Token.Text) + " | (Logica) \"];\n";
                    }
                    else if (hijos.ChildNodes.ElementAt(1).Token.Text == "==" || hijos.ChildNodes.ElementAt(1).Token.Text == "!=" || hijos.ChildNodes.ElementAt(1).Token.Text == "<" || hijos.ChildNodes.ElementAt(1).Token.Text == ">" || hijos.ChildNodes.ElementAt(1).Token.Text == "<=" || hijos.ChildNodes.ElementAt(1).Token.Text == ">=")
                    {
                        grafo = grafo + nombrehijo + "[ color=purple, label=\"" + escapar(hijos.ChildNodes.ElementAt(1).Token.Text) + " | (Relacional) \"];\n";
                    }
                    else
                    {
                        grafo = grafo + nombrehijo + "[color=purple, label=\"" + escapar(hijos.ChildNodes.ElementAt(1).Token.Text) + " | (Aritmetica) \"];\n";
                    }
                   
                }
                grafo = grafo + padre + "->" + nombrehijo + "; \n";
                contador++;
                recorrerEXP(nombrehijo, hijos.ChildNodes.ElementAt(0));
                recorrerEXP(nombrehijo, hijos.ChildNodes.ElementAt(2));

            }
            else if (hijos.ChildNodes.Count == 0)
            {
                String nombrehijo = "nodo" + contador.ToString();
                grafo = grafo + nombrehijo + "[color=purple, label=\"" + escapar(hijos.ToString()) + "\"];\n";
                grafo = grafo + padre + "->" + nombrehijo + "; \n";
                contador++;
            }
            else if (hijos.ChildNodes.ElementAt(0).ToString() == "LLAMADA")
            {
                String nombrehijo = "nodo" + contador.ToString();
                grafo = grafo + nombrehijo + "[color=purple, label=\"" + escapar(hijos.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Text) + " | (Funcion) \"];\n";
                grafo = grafo + padre + "->" + nombrehijo + "; \n";
                contador++;
            }
            else if (hijos.ChildNodes.Count == 2)
            {
                String nombrehijo = "nodo" + contador.ToString();
                if (hijos.ChildNodes.ElementAt(0).Token.Text == "!")
                {
                    grafo = grafo + nombrehijo + "[color=purple, label=\"" + escapar(hijos.ChildNodes.ElementAt(0).Token.Text) + " | (Logica) \"];\n";
                }
                else
                {
                    grafo = grafo + nombrehijo + "[color=purple, label=\"" + escapar(hijos.ChildNodes.ElementAt(0).Token.Text) + " | (Aritmetica) \"];\n";
                }
                grafo = grafo + padre + "->" + nombrehijo + "; \n";
                contador++;
                recorrerEXP(nombrehijo, hijos.ChildNodes.ElementAt(1));
            }
            else if (hijos.ChildNodes.Count == 1)
            {
                recorrerEXP(padre, hijos.ChildNodes.ElementAt(0));
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
