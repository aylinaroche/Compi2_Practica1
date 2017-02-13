using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBScript
{
    class Listas
    {
        public static int incerteza = 0;
        public static string ruta = "";
        public static ArrayList listaVariables = new ArrayList();
        public static ArrayList MensajeConsola = new ArrayList();


        public static Variable obtenerVariable(String nombre)
        {
            Variable v = null;
            for (int a = 0; a < Listas.listaVariables.Count; a++)
            {
                v = (Variable)Listas.listaVariables[a];
                String varNombre = v.nombre + " ";

                if (v.nombre == nombre || varNombre == nombre)
                {
                    return v;
                }
            }
            return null;
        }

  
    }
}
