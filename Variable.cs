using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBScript
{
    class Variable
    {
        public String nombre = "";
        public String tipo = "";
        public String ambito = "";
        public String valor = "";
    }

    class Variables
    {
        public static ArrayList listaVariables = new ArrayList();

        public static Boolean crearVariable(String tipo, String nombre, String valor, String ambito)
        {
            Variable v = null;
            for (int a = 0; a < listaVariables.Count; a++)
            {
                v = (Variable)listaVariables[a];
                if (v.nombre == nombre && v.tipo == tipo && v.ambito == ambito)
                {
                    Reporte.agregarMensajeError("La variable '" + v.nombre + "' ya existe", "Error Semantico", 0, 0);
                    return false;
                }
            }

            if (PrimerRecorrido.concatenar == false)//Numero
            {
                if (tipo == "String")
                {
                    Reporte.agregarMensajeError("Tipo incorrecto", "Error Semantico", 0, 0);
                    return false;
                }
                else if (tipo == "Bool")
                {
                    if (valor == "1")
                    {
                        valor = "true";
                    }
                    else if (valor == "0")
                    {
                        valor = "false";
                    }
                    else
                    {
                        Reporte.agregarMensajeError("Tipo Incorrecto", "Error Semantico", 0, 0);
                        return false;
                    }
                }
            }
            else //Cadena
            {
                if (tipo == "Number")
                {
                    Reporte.agregarMensajeError("Tipo incorrecto", "Error Semantico", 0, 0);
                    return false;
                }
                if (tipo == "Bool")
                {
                    if (!(valor == "1" || valor == "0" || valor == ""))
                    {
                        Reporte.agregarMensajeError("Tipo incorrecto", "Error Semantico", 0, 0);
                        return false;
                    }

                }
            }
            Variable nueva = new Variable();
            nueva.tipo = tipo;
            nueva.nombre = nombre;
            nueva.valor = valor;
            nueva.ambito = ambito;
            listaVariables.Add(nueva);
            return true;
        }

        public static Variable obtenerVariable(String nombre)
        {
            Variable v = null;
            for (int a = 0; a < listaVariables.Count; a++)
            {
                v = (Variable)listaVariables[a];
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
