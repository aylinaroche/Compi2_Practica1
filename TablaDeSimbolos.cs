using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBScript
{
    class TablaDeSimbolos
    {
        public static ArrayList Simbolos = new ArrayList();
        public static void agregarSimbolo(String t, String n, String v, String a, String r, String tam, String p)
        {
            bool existe = false;
            for (int i = 0; i < Simbolos.Count; i++)
            {
                Simbolo s = (Simbolo)Simbolos[i];
                if (s.nombre==(n) && s.tipo==(t) && s.rol==(r))
                {
                    existe = true;
                }
            }
            if (existe == true)
            {
              //  ejecutar.Errores.agregarError(n, "Error Semantico", "La variable ya existe", 0, 0);
            }
            else
            {
                Simbolo s = new Simbolo();
                s.nombre = n;
                s.ambito = a;
                s.rol = r;
                s.tamanio = tam;
                s.tipo = t;
                s.valor = v;
                Simbolos.Add(s);
            }
        }

        public static ArrayList metodoFuncion = new ArrayList();
        public static ArrayList parametros = new ArrayList();

        public static void agregarMF(String n, String t, String r, String nodo, ArrayList parametro)
        {
            MF mf = new MF();
            mf.nombre = n;
            mf.tipo = t;
            mf.retorno = r;
            mf.nodo = nodo;
            mf.parametro = (ArrayList)parametro.Clone();
            metodoFuncion.Add(mf);
            //System.out.println("");
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
                agregarSimbolo(p.tipo, p.nombre, "", ambito, "parametro", "-", "-");

            }
        }

        public static double raiz(double n, double e)
        {
            double d = 1;
            for (int i = 0; i < e; i++)
            {
                d = d * d;
            }
            return d;
        }

    }

}
