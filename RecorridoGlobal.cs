using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Windows.Forms;

namespace SBScript
{
    class RecorridoGlobal
    {
        public static int contador = 0;
        public static Boolean boolean = false;
        public static String tipo = "";
        public static Boolean continuar = false;
        public static Boolean detener = false;
        public static Boolean retornar = false;

        public static String action(ParseTreeNode nodo)
        {
            String result = null;
            String variables = "";
            switch (nodo.Term.Name.ToString())
            {
                case "INICIO":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            action(nodo.ChildNodes[0]);
                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            action(nodo.ChildNodes[0]);
                            action(nodo.ChildNodes[1]);
                        }
                        break;
                    }
                case "ENTRADA":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            Variables.pilaAmbito.Push("Global");
                            Variables.nivelAmbito = 0;
                            action(nodo.ChildNodes[0]);
                        }
                        break;
                    }
                case "ENCABEZADO":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            action(nodo.ChildNodes[0]);
                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            action(nodo.ChildNodes[0]);
                            action(nodo.ChildNodes[1]);
                        }
                        break;
                    }
                case "TIPO":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            String[] numero = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            result = numero[0];
                        }
                        break;
                    }
                case "DECLARACION":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            String tipo = action(nodo.ChildNodes[0]);
                            String vars = action(nodo.ChildNodes[1]);
                            String[] var = (vars.Split(','));
                            String ambito = Variables.pilaAmbito.Peek().ToString();
                            for (int i = 0; i < var.Length - 1; i++)
                            {
                                Variables.declararVariable(tipo, var[i], "", ambito);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 4 || nodo.ChildNodes.Count == 5)
                        {
                            String tipo = action(nodo.ChildNodes[0]);
                            String vars = action(nodo.ChildNodes[1]);
                            String[] var = (vars.Split(','));
                            String asig = action(nodo.ChildNodes[3]);
                            String ambito = Variables.pilaAmbito.Peek().ToString();
                            if (asig != "error")
                            {
                                for (int i = 0; i < var.Length - 1; i++)
                                {
                                    Variables.declararVariable(tipo, var[i], asig, ambito);
                                }
                            }
                            else
                            {
                                Reporte.agregarMensajeError("Error al declarar la expresion en una variable global", "Error Semantico", 0, 0);
                            }
                        }
                        break;
                    }
                case "VAR":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            String[] numero = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            variables += numero[0] + ",";
                            result = variables;
                        }
                        else if (nodo.ChildNodes.Count == 3)
                        {
                            result = action(nodo.ChildNodes[0]);
                            String[] dato = nodo.ChildNodes.ElementAt(2).ToString().Split(' ');
                            result += dato[0] + ",";
                        }
                        break;
                    }
                case "ASIGNACION":
                    {
                        if (nodo.ChildNodes.Count == 3 || nodo.ChildNodes.Count == 4)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            String asig = action(nodo.ChildNodes[2]);
                            String ambito = Variables.pilaAmbito.Peek().ToString();
                            Variables.asignarVariable(dato[0], asig, ambito);
                        }
                        break;
                    }
                case "INSTRUCCIONES":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            action(nodo.ChildNodes[0]);
                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            action(nodo.ChildNodes[0]);
                            action(nodo.ChildNodes[1]);
                        }
                        break;
                    }
                case "INSTRUCCION":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            action(nodo.ChildNodes[0]);
                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            //action(node.ChildNodes[0]);
                        }
                        break;
                    }
                case "RETORNAR":
                    {
                        // MessageBox.Show("TIPO");
                        if (nodo.ChildNodes.Count == 3)
                        {
                            action(nodo.ChildNodes[1]);
                        }
                        break;
                    }
                case "LLAMADA(":
                    {
                        Metodo_Funcion.parametrosTemp.Clear();
                        if (nodo.ChildNodes.Count == 3)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            Listas.nodoActual = nodo.ChildNodes[0];
                            ParseTreeNode metodo = Metodo_Funcion.buscarMetodo(dato[0]);
                            if (metodo != null)
                            {
                                Variables.pilaAmbito.Push(dato[0]);
                                Variables.nivelAmbito += 1;
                                SegundoRecorrido.action(metodo);
                                Variables.eliminarAmbito();
                            }
                            else
                            {
                                Reporte.agregarMensajeError("El metodo/funcion no existe", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            Listas.nodoActual = nodo.ChildNodes[0];
                            ParseTreeNode metodo = null;
                            if (nodo.ChildNodes[2].Term.Name.ToString() == "TipoPARAMETRO")
                            {
                                action(nodo.ChildNodes[2]);
                                metodo = Metodo_Funcion.buscarMetodo(dato[0]);
                                if (metodo != null)
                                {
                                    Variables.pilaAmbito.Push(dato[0]);
                                    Variables.nivelAmbito += 1;
                                    Metodo_Funcion.guardarParametro(dato[0]);
                                    SegundoRecorrido.action(metodo);
                                    Variables.eliminarAmbito();
                                }
                                else
                                {
                                    Reporte.agregarMensajeError("El metodo/funcion no existe", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                                }
                            }
                            else
                            {
                                metodo = Metodo_Funcion.buscarMetodo(dato[0]);

                                if (metodo != null)
                                {
                                    Variables.pilaAmbito.Push(dato[0]);
                                    Variables.nivelAmbito += 1;
                                    //  Metodo_Funcion.guardarParametro();
                                    SegundoRecorrido.action(metodo);
                                    Variables.eliminarAmbito();
                                }
                                else
                                {
                                    Reporte.agregarMensajeError("El metodo/funcion no existe", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                                }
                            }


                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            Listas.nodoActual = nodo.ChildNodes[0];
                            SegundoRecorrido.action(nodo.ChildNodes[2]);
                            ParseTreeNode metodo = Metodo_Funcion.buscarMetodo(dato[0]);
                            if (metodo != null)
                            {
                                Variables.pilaAmbito.Push(dato[0]);
                                Variables.nivelAmbito += 1;
                                Metodo_Funcion.guardarParametro(dato[0]);
                                SegundoRecorrido.action(metodo);
                                Variables.eliminarAmbito();
                            }
                            else
                            {
                                Reporte.agregarMensajeError("El metodo/funcion no existe", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                            }

                        }
                        Metodo_Funcion.parametrosTemp.Clear();
                        result = SegundoRecorrido.retorno;
                        SegundoRecorrido.retorno = "";
                        break;
                    }
                case "TipoPARAMETRO":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            result = action(nodo.ChildNodes[0]);
                            String tipo = "";
                            if (PrimerRecorrido.concatenar == true)
                            {
                                tipo = "String";
                            }
                            else //Number o Bool
                            {
                                if (result == "1" || result == "0")
                                {
                                    tipo = "Bool";
                                }
                                else
                                {
                                    tipo = "Number";
                                }
                            }
                            Metodo_Funcion.agregarParametroTemp(tipo, result);

                        }
                        else if (nodo.ChildNodes.Count == 3)
                        {
                            action(nodo.ChildNodes[0]);
                            result = action(nodo.ChildNodes[2]);
                            String tipo = "";
                            if (PrimerRecorrido.concatenar == true)
                            {
                                tipo = "String";
                            }
                            else //Number o Bool
                            {
                                if (result == "1" || result == "0")
                                {
                                    tipo = "Bool";
                                }
                                else
                                {
                                    tipo = "Number";
                                }
                            }
                            Metodo_Funcion.agregarParametroTemp(tipo, result);
                        }
                        break;
                    }
                case "EXPRESION":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            result = action(nodo.ChildNodes[0]);
                        }
                        break;
                    }
                case "OpARITMETICO":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            result = action(nodo.ChildNodes[0]);
                        }
                        break;
                    }
                case "E":
                    {
                        //    MessageBox.Show("E");
                        if (nodo.ChildNodes.Count == 1|| nodo.ChildNodes.Count == 2||nodo.ChildNodes.Count == 3)
                        {
                            result =SegundoRecorrido.resolverOperacion(nodo).ToString();

                        }
                        break;
                    }

                default:
                    break;
            }
            return result;
        }

      
    }
}
