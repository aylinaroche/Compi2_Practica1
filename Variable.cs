using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace SBScript
{
    class Variable
    {
        public String nombre = "";
        public String tipo = "";
        public String ambito = "";
        public String valor = "";
        public int nivel = 0;
    }

    class Variables
    {
        public static Stack pilaAmbito = new Stack();
        public static ArrayList listaVariables = new ArrayList();
        public static int nivelAmbito = 0;
        public static Boolean crearVariable(String tipo, String nombre, String valor, String ambito)
        {
            ParseTreeNode nodo = Listas.nodoActual;
            Variable v = null;
            for (int a = 0; a < listaVariables.Count; a++)
            {
                v = (Variable)listaVariables[a];
                if (v.nombre == nombre && v.tipo == tipo && v.ambito == ambito)
                {
                    Reporte.agregarMensajeError("La variable '" + v.nombre + "' ya existe", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                    return false;
                }
            }

            if (PrimerRecorrido.concatenar == false)//Numero
            {
                if (tipo == "String")
                {
                    Reporte.agregarMensajeError("Tipo incorrecto: '"+nombre+"'", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
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
                        Reporte.agregarMensajeError("Tipo Incorrecto '" + nombre + "'", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                        return false;
                    }
                }
            }
            else //Cadena
            {
                if (tipo == "Number")
                {
                    Reporte.agregarMensajeError("Tipo incorrecto '" + nombre + "'", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                    return false;
                }
                if (tipo == "Bool")
                {
                    if (!(valor == "1" || valor == "0" || valor == ""))
                    {
                        Reporte.agregarMensajeError("Tipo incorrecto '" + nombre + "'", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                        return false;
                    }

                }
            }
            Variable nueva = new Variable();
            nueva.tipo = tipo;
            nueva.nombre = nombre;
            nueva.valor = valor;
            nueva.ambito = ambito;
            nueva.nivel = nivelAmbito;
            listaVariables.Add(nueva);
            return true;
        }

        public static Boolean declararVariable(String tipo, String nombre, String valor, String ambito)
        {
            ParseTreeNode nodo = Listas.nodoActual;
            Variable v = null;
            for (int a = 0; a < listaVariables.Count; a++)
            {
                v = (Variable)listaVariables[a];
                if (v.nombre == nombre && v.tipo == tipo && v.ambito == ambito)
                {
                    Reporte.agregarMensajeError("La variable '" + v.nombre + "' ya existe", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                    return false;
                }
            }
            Variable nueva = new Variable();
            nueva.tipo = tipo;
            nueva.nombre = nombre;
            nueva.valor = valor;
            nueva.ambito = ambito;
            nueva.nivel = nivelAmbito;
            listaVariables.Add(nueva);
            return true;
        }

        public static Boolean asignarVariable(String nombre, String valor, String ambito)
        {
            ParseTreeNode nodo = Listas.nodoActual;
            for (int a = listaVariables.Count - 1; a > -1; a--)
            {
                Variable v = (Variable)listaVariables[a];
                if (v.nombre == nombre)
                {
                    if (PrimerRecorrido.concatenar == false)//Numero
                    {
                        if (v.tipo == "String")
                        {
                            Reporte.agregarMensajeError("Tipo incorrecto al asignar variable '" + nombre + "'", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                            return false;
                        }
                        else if (v.tipo == "Bool")
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
                                Reporte.agregarMensajeError("Tipo incorrecto al asignar variable '" + nombre + "'", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                                return false;
                            }
                        }
                    }
                    else //Cadena
                    {
                        if (v.tipo == "Number")
                        {
                            Reporte.agregarMensajeError("Tipo incorrecto al asignar variable '" + nombre + "'", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                            return false;
                        }
                        if (v.tipo == "Bool")
                        {
                            if (!(valor == "1" || valor == "0" || valor == ""))
                            {
                                Reporte.agregarMensajeError("Tipo incorrecto al asignar variable '" + nombre + "'", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                                return false;
                            }
                        }
                    }
                    v.valor = valor;
                    return true;
                }

            }

            return false;
        }

        public static Variable obtenerVariable(String nombre)
        {
            Variable v = null;
            for (int a = listaVariables.Count - 1; a > -1; a--)
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

        public static void eliminarAmbito()
        {
            int nivel = nivelAmbito;
            for (int i = listaVariables.Count - 1; i > -1; i--)
            {
                Variable v = (Variable)listaVariables[i];
                if (v.nivel == nivel)
                {
                    listaVariables.Remove(v);
                }
            }
            Variables.pilaAmbito.Pop();
            Variables.nivelAmbito -= 1;

        }

    }
}
