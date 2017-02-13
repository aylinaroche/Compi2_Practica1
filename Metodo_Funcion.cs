using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace SBScript
{
    class Metodo_Funcion
    {
        public static ArrayList metodoFuncion = new ArrayList();
        public static ArrayList parametros = new ArrayList();

        public static void agregarMF(String t, String n, String r, ParseTreeNode nodo,  ArrayList p)
        {

            ArrayList param = new ArrayList();
            
            Boolean existe = false;
            for(int i = 0; i < metodoFuncion.Count; i++)
            {
                MF m = (MF)metodoFuncion[i];
                if (m.tipo.Equals(t) && m.nombre.Equals(n) && p.Count==m.parametro.Count)
                    {
                    for (int j= 0; j < p.Count; j++)
                    {
                        if (p[j] == m.parametro[j])
                        {
                            existe=true;
                        }
                    }
                    if(parametros.Count == 0 && m.parametro.Count == 0)
                    {
                        existe = true;
                    }

                }

            }
            if (existe == false)
            {
                MF mf = new MF();
                mf.nombre = n;
                mf.tipo = t;
                mf.retorno = r;
                mf.nodo = nodo;
                mf.parametro = (ArrayList)parametros.Clone();
                metodoFuncion.Add(mf);
            }
            else
            {
                Reporte.agregarError("Ya existe un metodo o funcion llamada " + n, "Error Semantico", 0, 0);
                Listas.MensajeConsola.Add("> Ya existe un metodo o funcion llamada " + n + "\n");
            }

        }

        public static void agregarParametro(String t, String n)
        {
            Parametro p = new Parametro();
            p.nombre = n;
            p.tipo = t;
            parametros.Add(p);
        }

        public static void parametroSimbolo(String ambito)
        {

            for (int i = 0; i < parametros.Count; i++)
            {
                Parametro p = (Parametro)parametros[i];
               // agregarSimbolo(p.tipo, p.nombre, "", ambito, "parametro", "-", "-");

            }
        }

    }
    class MF
    {
        public String nombre;
        public String tipo;
        public String retorno;
        public ParseTreeNode nodo;
        public ArrayList parametro;

    }

    class Parametro
    {
        public String nombre;
        public String tipo;

    }

}
