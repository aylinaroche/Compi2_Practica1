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

  
    }

}
