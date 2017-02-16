using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace SBScript
{
    class SegundoRecorrido
    {
        public static int contador = 0;
        public static Boolean boolean = false;
        public static String sentencia = "";
        public static String retorno = "";
        public static String tipo = "";
        public static String action(ParseTreeNode nodo)
        {
            String result = null;
            String variables = "";
            switch (nodo.Term.Name.ToString())
            {
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
                                Variables.crearVariable(tipo, var[i], "", ambito);
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
                                    Variables.crearVariable(tipo, var[i], asig, ambito);
                                }
                            }
                            else
                            {
                                Reporte.agregarMensajeError("No puede declarar.", "Error Semantico", 0, 0);
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
                case "MOSTRAR":
                    {
                        contador = 0;
                        if (nodo.ChildNodes.Count == 5)
                        {
                            result = action(nodo.ChildNodes[2]);
                            Listas.MensajeConsola.Add(">> " + result + ".\n");
                        }
                        break;
                    }
                case "DatosMOSTRAR":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            result = action(nodo.ChildNodes[0]);
                        }
                        else if (nodo.ChildNodes.Count == 3)
                        {
                            String cadena = action(nodo.ChildNodes[0]);
                            String indice = "{" + contador + "}";
                            String expr = action(nodo.ChildNodes[2]);
                            try
                            {
                                cadena = cadena.Replace(indice, expr);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("EE");
                            }
                            result = cadena;
                            contador += 1;
                        }

                        break;
                    }

                case "DibujarAST":
                    {
                        if (nodo.ChildNodes.Count == 5)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(2).ToString().Split(' '));
                            ParseTreeNode metodo = Metodo_Funcion.buscarMetodo(dato[0]);
                            //   Analizador.generarAST(metodo, dato[0]);
                        }
                        break;
                    }
                case "DibujarEXP":
                    {
                        if (nodo.ChildNodes.Count == 5)
                        {
                            //result = action(node.ChildNodes[0]);
                        }
                        break;
                    }
                case "INSTRUCCIONES":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            result = action(nodo.ChildNodes[0]);
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
                            result = action(nodo.ChildNodes[0]);
                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            //action(node.ChildNodes[0]);
                        }
                        break;
                    }
                case "RETORNAR":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            //if (nodo.ChildNodes[1].Term.Name.ToString() == "OP")
                            //{
                                retorno = action(nodo.ChildNodes[1]);
                            //}
                            //else
                            //{
                                //retorno solo
                            //}
                        }
                        else if (nodo.ChildNodes.Count == 3)
                        {
                            retorno = action(nodo.ChildNodes[1]);
                        }
                        break;
                    }
                case "MAIN":
                    {
                        Metodo_Funcion.parametros.Clear();
                        if (nodo.ChildNodes.Count == 6)
                        {
                            Metodo_Funcion.agregarMF("MAIN", "MAIN", "", nodo.ChildNodes[4], null);
                        }
                        break;
                    }

                case "LLAMADA(":
                    {
                        Metodo_Funcion.parametrosTemp.Clear();
                        if (nodo.ChildNodes.Count == 3)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            ParseTreeNode metodo = Metodo_Funcion.buscarMetodo(dato[0]);
                            if (metodo != null)
                            {
                                Variables.pilaAmbito.Push(dato[0]);
                                Variables.nivelAmbito += 1;
                                action(metodo);
                                Variables.eliminarAmbito();
                            }
                            else
                            {
                                Reporte.agregarMensajeError("El metodo/funcion no existe", "Error Semantico", 0, 0);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
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
                                    action(metodo);
                                    Variables.eliminarAmbito();
                                }
                                else
                                {
                                    Reporte.agregarMensajeError("El metodo/funcion no existe", "Error Semantico", 0, 0);
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
                                    action(metodo);
                                    Variables.eliminarAmbito();
                                }
                                else
                                {
                                    Reporte.agregarMensajeError("El metodo/funcion no existe", "Error Semantico", 0, 0);
                                }
                            }


                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            action(nodo.ChildNodes[2]);
                            ParseTreeNode metodo = Metodo_Funcion.buscarMetodo(dato[0]);
                            if (metodo != null)
                            {
                                Variables.pilaAmbito.Push(dato[0]);
                                Variables.nivelAmbito += 1;
                                Metodo_Funcion.guardarParametro(dato[0]);
                                action(metodo);
                                Variables.eliminarAmbito();
                            }
                            else
                            {
                                Reporte.agregarMensajeError("El metodo/funcion no existe", "Error Semantico", 0, 0);
                            }

                        }
                        Metodo_Funcion.parametrosTemp.Clear();
                        result = retorno;
                        retorno = "";
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
                case "SI":
                    {
                        if (nodo.ChildNodes.Count == 7)
                        {
                            String restriccion = action(nodo.ChildNodes[2]);
                            if (restriccion == "true" || restriccion == "1")
                            {
                                action(nodo.ChildNodes[5]);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            String restriccion = action(nodo.ChildNodes[2]);
                            if (restriccion == "true" || restriccion == "1")
                            {
                                action(nodo.ChildNodes[5]);
                            }
                            else
                            {
                                action(nodo.ChildNodes[7]);
                            }
                        }
                        break;
                    }
                case "ELSE":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            action(nodo.ChildNodes[1]);
                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            action(nodo.ChildNodes[2]);
                        }
                        break;
                    }
                case "CICLO":
                    {
                        if (nodo.ChildNodes.Count == 7)
                        {
                            action(nodo.ChildNodes[2]);
                            action(nodo.ChildNodes[5]);
                        }
                        break;
                    }
                case "SWITCH":
                    {
                        boolean = false;
                        if (nodo.ChildNodes.Count == 5)
                        {
                            sentencia = action(nodo.ChildNodes[2]);
                            if (PrimerRecorrido.concatenar == false)//Numero
                            {
                                if (sentencia == "1" || sentencia == "0")
                                {
                                    Reporte.agregarMensajeError("Tipo Bool no permitido en Selecciona", "Error Semantico", 0, 0);
                                }
                                tipo = "Number";
                            }
                            else //Cadena
                            {
                                tipo = "String";
                            }
                            action(nodo.ChildNodes[4]);
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            sentencia = action(nodo.ChildNodes[2]);
                            result = action(nodo.ChildNodes[4]);
                            if (result == "false")
                            {
                                action(nodo.ChildNodes[5]);
                            }
                        }
                        break;
                    }
                case "CASE":
                    {
                        if (nodo.ChildNodes.Count == 5)
                        {
                            result = action(nodo.ChildNodes[0]);
                            if (result == sentencia)
                            {
                                boolean = true;

                            }
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            action(nodo.ChildNodes[3]);
                        }
                        break;
                    }
                case "DEFAULT":
                    {
                        if (nodo.ChildNodes.Count == 5)
                        {
                            action(nodo.ChildNodes[3]);
                        }
                        break;
                    }
                case "FOR":
                    {
                        if (nodo.ChildNodes.Count == 7)
                        {
                            action(nodo.ChildNodes[2]);
                            action(nodo.ChildNodes[5]);
                        }
                        break;
                    }
                case "PARA":
                    {
                        if (nodo.ChildNodes.Count == 8)
                        {
                            action(nodo.ChildNodes[3]);
                            action(nodo.ChildNodes[5]);
                            action(nodo.ChildNodes[7]);
                        }
                        break;
                    }
                case "OP":
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
                        if (nodo.ChildNodes.Count == 1)
                        {
                            result = resolverOperacion(nodo).ToString();
                        }
                        else if (nodo.ChildNodes.Count == 3)
                        {
                            result = resolverOperacion(nodo).ToString();

                        }
                        break;
                    }

                default:
                    break;
            }
            return result;


        }

        public static String resolverOperacion(ParseTreeNode root)
        {
            PrimerRecorrido.concatenar = false;
            String resultado = expresion(root);
            return resultado;
        }

        private static String expresion(ParseTreeNode root)
        {
            switch (root.ChildNodes.Count)
            {
                case 1:
                    String[] dato = (root.ChildNodes.ElementAt(0).ToString().Split('('));

                    if (dato[1] == "cadena)")
                    {
                        PrimerRecorrido.concatenar = true;
                        String cadena = dato[0].Substring(0, dato[0].Length - 1);
                        return cadena;
                    }
                    else if (dato[1] == "numero)")
                    {
                        PrimerRecorrido.concatenar = false;
                        return dato[0];
                    }
                    else if (dato[1] == "id)")
                    {
                        Variable v = Variables.obtenerVariable(dato[0]);
                        if (v != null)
                        {
                            if (v.tipo == "String")
                            {
                                PrimerRecorrido.concatenar = true;
                            }
                            return v.valor;
                        }
                        else
                        {
                            Reporte.agregarMensajeError("El id '" + dato[0] + "' no existe", "Error Semantico", 0, 0);
                            // MessageBox.Show("ERROR"); //No encuentra el id
                            PrimerRecorrido.concatenar = true;
                            return "";
                        }
                        //   return "";
                    }
                    else if (dato[1] == "Keyword)")
                    {
                        boolean = true;
                        PrimerRecorrido.concatenar = false;
                        if (dato[0] == "true " || dato[0] == "true")
                        {
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else if (dato[0] == "LLAMADA")
                    {
                        String r = action(root.ChildNodes[0]);
                        return r;
                    }
                    else
                    {
                        PrimerRecorrido.concatenar = true;
                        return "";
                    }
                case 2:
                    String res = action(root.ChildNodes[1]);
                    try
                    {
                        if (PrimerRecorrido.concatenar == false)
                        {
                            Double v1 = Convert.ToDouble(res);
                            v1 = v1 * (-1);
                            res = v1.ToString();
                            return res;
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("E");
                    }
                    return "";

                case 3: //Nodo binario
                    String resultado = "";
                    String E1 = "";
                    String E2 = "";
                    Double var1, var2;
                    String s = root.ChildNodes.ElementAt(1).ToString().Substring(0, 2);
                    switch (s)
                    {

                        case "+ ": //E+E
                            boolean = false;
                            E1 = expresion(root.ChildNodes.ElementAt(0));
                            Boolean c1 = PrimerRecorrido.concatenar;
                            Boolean b1 = boolean;
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            Boolean c2 = PrimerRecorrido.concatenar;
                            Boolean b2 = boolean;
                            if (b1 == false || b2 == false)
                            {
                                if (c1 == true || c2 == true)
                                {
                                    resultado = E1 + E2;
                                    PrimerRecorrido.concatenar = true;
                                    return resultado;
                                }
                                else
                                {
                                    try
                                    {
                                        Double v1 = Convert.ToDouble(E1);
                                        Double v2 = Convert.ToDouble(E2);
                                        Double r = v1 + v2;
                                        resultado = r.ToString();
                                        PrimerRecorrido.concatenar = false;
                                        return resultado;

                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("E");
                                        return "";
                                    }
                                }
                            }
                            else
                            {
                                if (E1 == "1" || E2 == "1")
                                {
                                    return "1";
                                }
                                return "0";
                            }


                        case "- ": //E-E
                            E1 = expresion(root.ChildNodes.ElementAt(0));
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            try
                            {
                                Double r = Convert.ToDouble(E1) - Convert.ToDouble(E2);
                                resultado = r.ToString();
                                return resultado;

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("E");
                                return "";
                            }
                        case "/ ": //E/E
                            E1 = expresion(root.ChildNodes.ElementAt(0));
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            try
                            {
                                Double r = Convert.ToDouble(E1) / Convert.ToDouble(E2);
                                resultado = r.ToString();
                                if (resultado == "NaN" || resultado == "∞")
                                {
                                    Reporte.agregarMensajeError("Error al dividir con '0'", "Error Ejecucion", 0, 0);
                                    return "0";
                                }
                                return resultado;

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("E");
                                return "";
                            }
                        case "* ": //E*E
                            E1 = expresion(root.ChildNodes.ElementAt(0));
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            try
                            {
                                Double r = Convert.ToDouble(E1) * Convert.ToDouble(E2);
                                resultado = r.ToString();
                                return resultado;

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("E");
                                return "";
                            }
                        case "% ": //E*E
                            E1 = expresion(root.ChildNodes.ElementAt(0));
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            try
                            {
                                Double r = Convert.ToDouble(E1) % Convert.ToDouble(E2);
                                resultado = r.ToString();
                                return resultado;

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("E");
                                return "";
                            }

                        case "> ":
                            try
                            {
                                E1 = expresion(root.ChildNodes.ElementAt(0));
                                c1 = PrimerRecorrido.concatenar;
                                E2 = expresion(root.ChildNodes.ElementAt(2));
                                c2 = PrimerRecorrido.concatenar;
                                if (c1 == true || c2 == true)
                                {
                                    PrimerRecorrido.concatenar = true;
                                    if (Listas.compararCadenas(E1, E2))
                                    {
                                        return "1";
                                    }
                                    return "0";
                                }
                                else
                                {
                                    try
                                    {
                                        Double v1 = Convert.ToDouble(E1);
                                        Double v2 = Convert.ToDouble(E2);
                                        PrimerRecorrido.concatenar = false;
                                        if (v1 > v2)
                                        {
                                            return "1";
                                        }
                                        return "0";
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("E");
                                        return "";
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("E");
                                return "error";
                            }

                        case "< ":
                            try
                            {
                                E1 = expresion(root.ChildNodes.ElementAt(0));
                                c1 = PrimerRecorrido.concatenar;
                                E2 = expresion(root.ChildNodes.ElementAt(2));
                                c2 = PrimerRecorrido.concatenar;
                                if (c1 == true || c2 == true)
                                {
                                    PrimerRecorrido.concatenar = true;
                                    if (Listas.compararCadenas(E1, E2))
                                    {
                                        return "0";
                                    }
                                    return "1";
                                }
                                else
                                {
                                    try
                                    {
                                        Double v1 = Convert.ToDouble(E1);
                                        Double v2 = Convert.ToDouble(E2);
                                        PrimerRecorrido.concatenar = false;
                                        if (v1 < v2)
                                        {
                                            return "1";
                                        }
                                        return "0";
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("E");
                                        return "";
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("E");
                                return "error";
                            }
                        case ">=":
                            try
                            {
                                E1 = expresion(root.ChildNodes.ElementAt(0));
                                c1 = PrimerRecorrido.concatenar;
                                E2 = expresion(root.ChildNodes.ElementAt(2));
                                c2 = PrimerRecorrido.concatenar;
                                if (c1 == true || c2 == true)
                                {
                                    PrimerRecorrido.concatenar = true;
                                    if (Listas.compararCadenas(E1, E2))
                                    {
                                        return "1";
                                    }
                                    return "0";
                                }
                                else
                                {
                                    try
                                    {
                                        Double v1 = Convert.ToDouble(E1);
                                        Double v2 = Convert.ToDouble(E2);
                                        PrimerRecorrido.concatenar = false;
                                        if (v1 >= v2)
                                        {
                                            return "1";
                                        }
                                        return "0";
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("E");
                                        return "";
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("E");
                                return "error";
                            }
                        case "<=":
                            try
                            {
                                E1 = expresion(root.ChildNodes.ElementAt(0));
                                c1 = PrimerRecorrido.concatenar;
                                E2 = expresion(root.ChildNodes.ElementAt(2));
                                c2 = PrimerRecorrido.concatenar;
                                if (c1 == true || c2 == true)
                                {
                                    PrimerRecorrido.concatenar = true;
                                    if (Listas.compararCadenas(E1, E2))
                                    {
                                        return "0";
                                    }
                                    return "1";
                                }
                                else
                                {
                                    try
                                    {
                                        Double v1 = Convert.ToDouble(E1);
                                        Double v2 = Convert.ToDouble(E2);
                                        PrimerRecorrido.concatenar = false;
                                        if (v1 <= v2)
                                        {
                                            return "1";
                                        }
                                        return "0";
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("E");
                                        return "";
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("E");
                                return "error";
                            }
                        case "==":
                            try
                            {
                                E1 = expresion(root.ChildNodes.ElementAt(0));
                                c1 = PrimerRecorrido.concatenar;
                                E2 = expresion(root.ChildNodes.ElementAt(2));
                                c2 = PrimerRecorrido.concatenar;
                                if (c1 == true || c2 == true)
                                {
                                    PrimerRecorrido.concatenar = true;
                                    if (Listas.compararCadenas(E1, E2))
                                    {
                                        return "1";
                                    }
                                    return "0";
                                }
                                else
                                {
                                    try
                                    {
                                        Double v1 = Convert.ToDouble(E1);
                                        Double v2 = Convert.ToDouble(E2);
                                        PrimerRecorrido.concatenar = false;
                                        if (v1 == v2)
                                        {
                                            return "1";
                                        }
                                        return "0";
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("E");
                                        return "";
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("E");
                                return "error";
                            }
                        case "!=":
                            try
                            {
                                E1 = expresion(root.ChildNodes.ElementAt(0));
                                c1 = PrimerRecorrido.concatenar;
                                E2 = expresion(root.ChildNodes.ElementAt(2));
                                c2 = PrimerRecorrido.concatenar;
                                if (c1 == true || c2 == true)
                                {
                                    PrimerRecorrido.concatenar = true;
                                    if (Listas.compararCadenas(E1, E2))
                                    {
                                        return "false";
                                    }
                                    return "true";
                                }
                                else
                                {
                                    try
                                    {
                                        Double v1 = Convert.ToDouble(E1);
                                        Double v2 = Convert.ToDouble(E2);
                                        PrimerRecorrido.concatenar = false;
                                        if (v1 != v2)
                                        {
                                            return "1";
                                        }
                                        return "0";
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("E");
                                        return "";
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("E");
                                return "error";
                            }
                        case "~ ":
                            try
                            {
                                E1 = expresion(root.ChildNodes.ElementAt(0));
                                c1 = PrimerRecorrido.concatenar;
                                E2 = expresion(root.ChildNodes.ElementAt(2));
                                c2 = PrimerRecorrido.concatenar;
                                if (c1 == true || c2 == true)
                                {
                                    PrimerRecorrido.concatenar = true;
                                    if (Listas.semejarCadenas(E1.ToLower(), E2.ToLower()))
                                    {
                                        return "1";
                                    }
                                    return "0";
                                }
                                else
                                {
                                    try
                                    {
                                        Double v1 = Convert.ToDouble(E1);
                                        Double v2 = Convert.ToDouble(E2);
                                        PrimerRecorrido.concatenar = false;
                                        Double abs = Math.Abs(v1 - v2);
                                        if (abs < Listas.incerteza)
                                        {
                                            return "1";
                                        }
                                        return "0";
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("E");
                                        return "";
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("E");
                                return "error";
                            }
                        case "&&":

                            E1 = expresion(root.ChildNodes.ElementAt(0));
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            try
                            {
                                Double r = Convert.ToDouble(E1) * Convert.ToDouble(E2);
                                if (E1 == "1" && E2 == "1" || E1 == "0" && E2 == "0")
                                {
                                    return "1";
                                }
                                else
                                {
                                    return "0";
                                }

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("E");
                                return "";
                            }

                        case "||":
                            E1 = expresion(root.ChildNodes.ElementAt(0));
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            try
                            {
                                if (E1 == "1" || E2 == "1")
                                {
                                    return "1";
                                }
                                else
                                {
                                    return "0";
                                }

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("E");
                                return "";
                            }

                        default: //(E)
                            return "0";
                    }

            }
            return "";
        }

    }
}
