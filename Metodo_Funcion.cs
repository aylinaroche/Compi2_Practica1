﻿using System;
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
        public static ArrayList listaMetodoFuncion = new ArrayList();
        public static ArrayList parametros = new ArrayList();

        public static void agregarMF(String t, String n, String r, ParseTreeNode nodo, ArrayList p)
        {
            Boolean existe = false;
            for (int i = 0; i < listaMetodoFuncion.Count; i++)
            {
                MF m = (MF)listaMetodoFuncion[i];
                if (m.tipo.Equals(t) && m.nombre.Equals(n) && p.Count == m.parametro.Count)
                {
                    for (int j = 0; j < p.Count; j++)
                    {
                        Parametro p1 = (Parametro)p[j];
                        Parametro p2 = (Parametro)m.parametro[j];
                        if (p1.nombre == p2.nombre && p1.tipo == p2.tipo)
                        {
                            existe = true;
                        }
                    }
                    if (parametros.Count == 0 && m.parametro.Count == 0)
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
                listaMetodoFuncion.Add(mf);
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

        public static ParseTreeNode buscarMetodo(String nombre)
        {
            ParseTreeNode nodo=null;
            for (int i = 0; i < listaMetodoFuncion.Count; i++)
            {
                MF mf = (MF)listaMetodoFuncion[i];
                if(mf.nombre == nombre)
                {
                    return mf.nodo;
                }
            }
            return nodo;
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