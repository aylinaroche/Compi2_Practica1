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

namespace SBScript
{
    class PrimerRecorrido
    {
        public static String mensajeAux = "";
        public static Boolean concatenar = false;
        public static Boolean boolean = false;

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
                case "C":
                    if (nodo.ChildNodes.Count == 2)
                    {
                        String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split('('));

                        if (dato[1] == "numero)")
                        {
                            Listas.incerteza = Double.Parse(dato[0]);
                        }
                        else if (dato[1] == "cadena)")
                        {
                            String ruta = dato[0];
                            if (Directory.Exists(ruta))
                            {
                                Listas.ruta = ruta;
                            }
                            else
                            {
                                Reporte.agregarError("No existe ruta: " + ruta, "Error General", 0, 0);
                              //  Listas.ruta = "C:/Users/Aylin/Documents/Visual Studio 2015/Projects/SBScript";
                            }
                        }
                    }
                    else if (nodo.ChildNodes.Count == 3)
                    {
                        Listas.incluir.Push(Listas.archivo.Peek().ToString());
                        Listas.nodoActual = nodo.ChildNodes[0];
                        String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                        Form1 f = new Form1();
                        f.incluirArchivo(dato[0] + ".sbs");
                        Listas.incluir.Pop();
                    }
                    break;

                case "TIPO":
                    {
                        if (nodo.ChildNodes.Count == 1)
                        {
                            String[] numero = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            result = numero[0];
                      }
                        break;
                    }
                case "MOSTRAR":
                    {
                        if (nodo.ChildNodes.Count == 5)
                        {
                            Reporte.agregarMensajeError("No puede imprimir en el area de variables globales", "Error Semantico", 0, 0);
                        }
                        break;
                    }

                case "DIBUJAR":
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
                case "METODO":
                    {
                        Metodo_Funcion.parametros.Clear();
                        ArrayList lista = new ArrayList();
                        if (nodo.ChildNodes.Count == 6)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                            Listas.nodoActual = nodo.ChildNodes[1];
                            Metodo_Funcion.agregarMF("Void", dato[0], "", null, lista);
                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                            Listas.nodoActual = nodo.ChildNodes[1];
                            Metodo_Funcion.agregarMF("Void", dato[0], "", nodo.ChildNodes[5], lista);
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            action(nodo.ChildNodes[3]);
                            Listas.nodoActual = nodo.ChildNodes[1];
                            String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                            Metodo_Funcion.agregarMF("Void", dato[0], "", nodo.ChildNodes[6], Metodo_Funcion.parametros);
                        }
                        break;
                    }
                case "FUNCION":
                    {
                        Metodo_Funcion.parametros.Clear();
                        ArrayList lista = new ArrayList();
                        if (nodo.ChildNodes.Count == 6)
                        {
                            String tipo = action(nodo.ChildNodes[0]);
                            String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                            Listas.nodoActual = nodo.ChildNodes[1];
                            Metodo_Funcion.agregarMF(tipo, dato[0], "", null, lista);
                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            String tipo = action(nodo.ChildNodes[0]);
                            String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                            Listas.nodoActual = nodo.ChildNodes[1];
                            if (nodo.ChildNodes[3].Term.Name.ToString() == "varPARAMETRO")
                            {
                                action(nodo.ChildNodes[3]);
                                Metodo_Funcion.agregarMF(tipo, dato[0], "", null, Metodo_Funcion.parametros);
                            }
                            else
                            {
                                Metodo_Funcion.agregarMF(tipo, dato[0], "", nodo.ChildNodes[5], lista);
                            }

                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            action(nodo.ChildNodes[3]);
                            String tipo = action(nodo.ChildNodes[0]);
                            String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                            Metodo_Funcion.agregarMF(tipo, dato[0], "", nodo.ChildNodes[6], Metodo_Funcion.parametros);
                        }

                        break;
                    }
                case "varPARAMETRO":
                    {
                        if (nodo.ChildNodes.Count == 2) //
                        {
                            String t = action(nodo.ChildNodes[0]);
                            String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                            Metodo_Funcion.agregarParametro(t, dato[0]);
                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            action(nodo.ChildNodes[0]);
                            String t = action(nodo.ChildNodes[2]);
                            String[] dato = (nodo.ChildNodes.ElementAt(3).ToString().Split(' '));
                            Metodo_Funcion.agregarParametro(t, dato[0]);
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
                case "MAIN":
                    {
                        Metodo_Funcion.parametros.Clear();
                        ArrayList vacio = new ArrayList();
                        if (nodo.ChildNodes.Count == 6)
                        {
                            Listas.nodoActual = nodo.ChildNodes[0];
                            if (Listas.incluir.Count == 0)
                            {
                                Metodo_Funcion.agregarMF("MAIN", "MAIN", "", nodo.ChildNodes[4], vacio);
                            }
                        }
                        break;
                    }

                case "LLAMADA":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            action(nodo.ChildNodes[2]);
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            result = action(nodo.ChildNodes[2]);
                        }
                        break;
                    }
                case "TipoPARAMETRO":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {

                        }
                        break;
                    }
                case "SI":
                    {
                        if (nodo.ChildNodes.Count == 7)
                        {
                            action(nodo.ChildNodes[2]);
                            action(nodo.ChildNodes[5]);
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            action(nodo.ChildNodes[2]);
                            action(nodo.ChildNodes[5]);
                            action(nodo.ChildNodes[7]);
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
                        if (nodo.ChildNodes.Count == 5)
                        {
                            action(nodo.ChildNodes[2]);
                            action(nodo.ChildNodes[4]);
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            action(nodo.ChildNodes[2]);
                            action(nodo.ChildNodes[4]);
                            action(nodo.ChildNodes[5]);
                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            action(nodo.ChildNodes[2]);
                            action(nodo.ChildNodes[5]);
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
                        //if (nodo.ChildNodes.Count == 1)
                        //{
                        //    result = resolverOperacion(nodo).ToString();
                        //}
                        //else if (nodo.ChildNodes.Count == 3)
                        //{
                        //    result = resolverOperacion(nodo).ToString();

                        //}
                        break;
                    }

                default:
                    break;
            }
            return result;
        }

        //public static String resolverOperacion(ParseTreeNode root)
        //{
        //    concatenar = false;
        //    String resultado = expresion(root);
        //    return resultado;
        //}

        //private static String expresion(ParseTreeNode root)
        //{
        //    switch (root.ChildNodes.Count)
        //    {
        //        case 1:
        //            String[] dato = (root.ChildNodes.ElementAt(0).ToString().Split('('));
        //            Listas.nodoActual = root.ChildNodes[0];
        //            if (dato[1] == "cadena)")
        //            {
        //                concatenar = true;
        //                String cadena = dato[0].Substring(0, dato[0].Length - 1);
        //                return cadena;
        //            }
        //            else if (dato[1] == "numero)")
        //            {
        //                concatenar = false;
        //                return dato[0];
        //            }
        //            else if (dato[1] == "id)")
        //            {
        //                Variable v = Variables.obtenerVariable(dato[0]);
        //                if (v != null)
        //                {
        //                    if (v.tipo == "string")
        //                    {
        //                        concatenar = true;
        //                    }
        //                    return v.valor;
        //                }
        //                else
        //                {
        //                    Reporte.agregarMensajeError("El id '" + dato[0] + "' no existe", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
        //                    concatenar = true;
        //                    return "error";
        //                }

        //            }
        //            else if (dato[1] == "Keyword)")
        //            {
        //                boolean = true;
        //                concatenar = false;
        //                if (dato[0] == "true " || dato[0] == "true")
        //                {
        //                    return "1";
        //                }
        //                else
        //                {
        //                    return "0";
        //                }
        //            }
        //            else if (dato[0] == "LLAMADA")
        //            {
        //                return "error";
        //            }
        //            else
        //            {
        //                concatenar = true;
        //                return "";
        //            }

        //        case 2:
        //            String res = action(root.ChildNodes[1]);
        //            try
        //            {
        //                if (concatenar == false)
        //                {
        //                    Double v1 = Convert.ToDouble(res);
        //                    v1 = v1 * (-1);
        //                    res = v1.ToString();
        //                    return res;
        //                }
        //                else
        //                {
        //                    return "0";
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                MessageBox.Show("E");
        //            }
        //            return "";

        //        case 3: //Nodo binario
        //            String resultado = "";
        //            String E1 = "";
        //            String E2 = "";
        //            Double var1, var2;

        //            switch (root.ChildNodes.ElementAt(1).ToString().Substring(0, 1))
        //            {

        //                case "+": //E+E
        //                    boolean = false;
        //                    E1 = expresion(root.ChildNodes.ElementAt(0));
        //                    Boolean c1 = PrimerRecorrido.concatenar;
        //                    Boolean b1 = boolean;
        //                    E2 = expresion(root.ChildNodes.ElementAt(2));
        //                    Boolean c2 = PrimerRecorrido.concatenar;
        //                    Boolean b2 = boolean;
        //                    if (b1 == false || b2 == false)
        //                    {
        //                        if (c1 == true || c2 == true)
        //                        {
        //                            resultado = E1 + E2;
        //                            PrimerRecorrido.concatenar = true;
        //                            return resultado;
        //                        }
        //                        else
        //                        {
        //                            try
        //                            {
        //                                Double v1 = Convert.ToDouble(E1);
        //                                Double v2 = Convert.ToDouble(E2);
        //                                Double r = v1 + v2;
        //                                resultado = r.ToString();
        //                                PrimerRecorrido.concatenar = false;
        //                                return resultado;

        //                            }
        //                            catch (Exception e)
        //                            {
        //                                MessageBox.Show("E");
        //                                return "";
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (E1 == "1" || E2 == "1")
        //                        {
        //                            return "1";
        //                        }
        //                        return "0";
        //                    }

        //                case "-": //E-E
        //                    E1 = expresion(root.ChildNodes.ElementAt(0));
        //                    E2 = expresion(root.ChildNodes.ElementAt(2));
        //                    try
        //                    {
        //                        Double r = Convert.ToDouble(E1) - Convert.ToDouble(E2); ;
        //                        resultado = r.ToString();
        //                        return resultado;

        //                    }
        //                    catch (Exception e)
        //                    {
        //                        return "error";
        //                        return "";
        //                    }
        //                case "/": //E/E
        //                    E1 = expresion(root.ChildNodes.ElementAt(0));
        //                    E2 = expresion(root.ChildNodes.ElementAt(2));
        //                    try
        //                    {
        //                        Double r = Convert.ToDouble(E1) / Convert.ToDouble(E2);
        //                        resultado = r.ToString();
        //                        if (resultado == "NaN" || resultado == "∞")
        //                        {
        //                            Reporte.agregarMensajeError("Error al dividir con '0'", "Error Ejecucion", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
        //                            return "0";
        //                        }
        //                        return resultado;

        //                    }
        //                    catch (Exception e)
        //                    {
        //                        MessageBox.Show("E");
        //                        return "";
        //                    }
        //                case "*": //E*E
        //                    E1 = expresion(root.ChildNodes.ElementAt(0));
        //                    E2 = expresion(root.ChildNodes.ElementAt(2));
        //                    try
        //                    {
        //                        Double r = Convert.ToDouble(E1) * Convert.ToDouble(E2);
        //                        resultado = r.ToString();
        //                        return resultado;

        //                    }
        //                    catch (Exception e)
        //                    {
        //                        MessageBox.Show("E");
        //                        return "";
        //                    }
        //                case "% ": //E*E
        //                    E1 = expresion(root.ChildNodes.ElementAt(0));
        //                    E2 = expresion(root.ChildNodes.ElementAt(2));
        //                    try
        //                    {
        //                        Double r = Convert.ToDouble(E1) % Convert.ToDouble(E2);
        //                        resultado = r.ToString();
        //                        return resultado;

        //                    }
        //                    catch (Exception e)
        //                    {
        //                        MessageBox.Show("E");
        //                        return "";
        //                    }

        //                case "> ":
        //                    try
        //                    {
        //                        E1 = expresion(root.ChildNodes.ElementAt(0));
        //                        c1 = PrimerRecorrido.concatenar;
        //                        E2 = expresion(root.ChildNodes.ElementAt(2));
        //                        c2 = PrimerRecorrido.concatenar;
        //                        if (c1 == true || c2 == true)
        //                        {
        //                            PrimerRecorrido.concatenar = true;
        //                            if (Listas.compararCadenas(E1, E2))
        //                            {
        //                                return "1";
        //                            }
        //                            return "0";
        //                        }
        //                        else
        //                        {
        //                            try
        //                            {
        //                                Double v1 = Convert.ToDouble(E1);
        //                                Double v2 = Convert.ToDouble(E2);
        //                                PrimerRecorrido.concatenar = false;
        //                                if (v1 > v2)
        //                                {
        //                                    return "1";
        //                                }
        //                                return "0";
        //                            }
        //                            catch (Exception e)
        //                            {
        //                                MessageBox.Show("E");
        //                                return "";
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        MessageBox.Show("E");
        //                        return "error";
        //                    }

        //                case "< ":
        //                    try
        //                    {
        //                        E1 = expresion(root.ChildNodes.ElementAt(0));
        //                        c1 = PrimerRecorrido.concatenar;
        //                        E2 = expresion(root.ChildNodes.ElementAt(2));
        //                        c2 = PrimerRecorrido.concatenar;
        //                        if (c1 == true || c2 == true)
        //                        {
        //                            PrimerRecorrido.concatenar = true;
        //                            if (Listas.compararCadenas(E1, E2))
        //                            {
        //                                return "0";
        //                            }
        //                            return "1";
        //                        }
        //                        else
        //                        {
        //                            try
        //                            {
        //                                Double v1 = Convert.ToDouble(E1);
        //                                Double v2 = Convert.ToDouble(E2);
        //                                PrimerRecorrido.concatenar = false;
        //                                if (v1 < v2)
        //                                {
        //                                    return "1";
        //                                }
        //                                return "0";
        //                            }
        //                            catch (Exception e)
        //                            {
        //                                MessageBox.Show("E");
        //                                return "";
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        MessageBox.Show("E");
        //                        return "error";
        //                    }
        //                case ">=":
        //                    try
        //                    {
        //                        E1 = expresion(root.ChildNodes.ElementAt(0));
        //                        c1 = PrimerRecorrido.concatenar;
        //                        E2 = expresion(root.ChildNodes.ElementAt(2));
        //                        c2 = PrimerRecorrido.concatenar;
        //                        if (c1 == true || c2 == true)
        //                        {
        //                            PrimerRecorrido.concatenar = true;
        //                            if (Listas.compararCadenas(E1, E2))
        //                            {
        //                                return "1";
        //                            }
        //                            return "0";
        //                        }
        //                        else
        //                        {
        //                            try
        //                            {
        //                                Double v1 = Convert.ToDouble(E1);
        //                                Double v2 = Convert.ToDouble(E2);
        //                                PrimerRecorrido.concatenar = false;
        //                                if (v1 >= v2)
        //                                {
        //                                    return "1";
        //                                }
        //                                return "0";
        //                            }
        //                            catch (Exception e)
        //                            {
        //                                MessageBox.Show("E");
        //                                return "";
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        MessageBox.Show("E");
        //                        return "error";
        //                    }
        //                case "<=":
        //                    try
        //                    {
        //                        E1 = expresion(root.ChildNodes.ElementAt(0));
        //                        c1 = PrimerRecorrido.concatenar;
        //                        E2 = expresion(root.ChildNodes.ElementAt(2));
        //                        c2 = PrimerRecorrido.concatenar;
        //                        if (c1 == true || c2 == true)
        //                        {
        //                            PrimerRecorrido.concatenar = true;
        //                            if (Listas.compararCadenas(E1, E2))
        //                            {
        //                                return "0";
        //                            }
        //                            return "1";
        //                        }
        //                        else
        //                        {
        //                            try
        //                            {
        //                                Double v1 = Convert.ToDouble(E1);
        //                                Double v2 = Convert.ToDouble(E2);
        //                                PrimerRecorrido.concatenar = false;
        //                                if (v1 <= v2)
        //                                {
        //                                    return "1";
        //                                }
        //                                return "0";
        //                            }
        //                            catch (Exception e)
        //                            {
        //                                MessageBox.Show("E");
        //                                return "";
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        MessageBox.Show("E");
        //                        return "error";
        //                    }
        //                case "==":
        //                    try
        //                    {
        //                        E1 = expresion(root.ChildNodes.ElementAt(0));
        //                        c1 = PrimerRecorrido.concatenar;
        //                        E2 = expresion(root.ChildNodes.ElementAt(2));
        //                        c2 = PrimerRecorrido.concatenar;
        //                        if (c1 == true || c2 == true)
        //                        {
        //                            PrimerRecorrido.concatenar = true;
        //                            if (Listas.compararCadenas(E1, E2))
        //                            {
        //                                return "1";
        //                            }
        //                            return "0";
        //                        }
        //                        else
        //                        {
        //                            try
        //                            {
        //                                Double v1 = Convert.ToDouble(E1);
        //                                Double v2 = Convert.ToDouble(E2);
        //                                PrimerRecorrido.concatenar = false;
        //                                if (v1 == v2)
        //                                {
        //                                    return "1";
        //                                }
        //                                return "0";
        //                            }
        //                            catch (Exception e)
        //                            {
        //                                MessageBox.Show("E");
        //                                return "";
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        MessageBox.Show("E");
        //                        return "error";
        //                    }
        //                case "!=":
        //                    try
        //                    {
        //                        E1 = expresion(root.ChildNodes.ElementAt(0));
        //                        c1 = PrimerRecorrido.concatenar;
        //                        E2 = expresion(root.ChildNodes.ElementAt(2));
        //                        c2 = PrimerRecorrido.concatenar;
        //                        if (c1 == true || c2 == true)
        //                        {
        //                            PrimerRecorrido.concatenar = true;
        //                            if (Listas.compararCadenas(E1, E2))
        //                            {
        //                                return "false";
        //                            }
        //                            return "true";
        //                        }
        //                        else
        //                        {
        //                            try
        //                            {
        //                                Double v1 = Convert.ToDouble(E1);
        //                                Double v2 = Convert.ToDouble(E2);
        //                                PrimerRecorrido.concatenar = false;
        //                                if (v1 != v2)
        //                                {
        //                                    return "1";
        //                                }
        //                                return "0";
        //                            }
        //                            catch (Exception e)
        //                            {
        //                                MessageBox.Show("E");
        //                                return "";
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        MessageBox.Show("E");
        //                        return "error";
        //                    }
        //                case "~ ":
        //                    try
        //                    {
        //                        E1 = expresion(root.ChildNodes.ElementAt(0));
        //                        c1 = PrimerRecorrido.concatenar;
        //                        E2 = expresion(root.ChildNodes.ElementAt(2));
        //                        c2 = PrimerRecorrido.concatenar;
        //                        if (c1 == true || c2 == true)
        //                        {
        //                            PrimerRecorrido.concatenar = true;
        //                            if (Listas.semejarCadenas(E1.ToLower(), E2.ToLower()))
        //                            {
        //                                return "1";
        //                            }
        //                            return "0";
        //                        }
        //                        else
        //                        {
        //                            try
        //                            {
        //                                Double v1 = Convert.ToDouble(E1);
        //                                Double v2 = Convert.ToDouble(E2);
        //                                PrimerRecorrido.concatenar = false;
        //                                Double abs = Math.Abs(v1 - v2);
        //                                if (abs < Listas.incerteza)
        //                                {
        //                                    return "1";
        //                                }
        //                                return "0";
        //                            }
        //                            catch (Exception e)
        //                            {
        //                                MessageBox.Show("E");
        //                                return "";
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        MessageBox.Show("E");
        //                        return "error";
        //                    }
        //                case "&&":

        //                    E1 = expresion(root.ChildNodes.ElementAt(0));
        //                    E2 = expresion(root.ChildNodes.ElementAt(2));
        //                    try
        //                    {
        //                        Double r = Convert.ToDouble(E1) * Convert.ToDouble(E2);
        //                        if (E1 == "1" && E2 == "1" || E1 == "0" && E2 == "0")
        //                        {
        //                            return "1";
        //                        }
        //                        else
        //                        {
        //                            return "0";
        //                        }

        //                    }
        //                    catch (Exception e)
        //                    {
        //                        MessageBox.Show("E");
        //                        return "";
        //                    }

        //                case "||":
        //                    E1 = expresion(root.ChildNodes.ElementAt(0));
        //                    E2 = expresion(root.ChildNodes.ElementAt(2));
        //                    try
        //                    {
        //                        if (E1 == "1" || E2 == "1")
        //                        {
        //                            return "1";
        //                        }
        //                        else
        //                        {
        //                            return "0";
        //                        }

        //                    }
        //                    catch (Exception e)
        //                    {
        //                        MessageBox.Show("E");
        //                        return "";
        //                    }


        //                default: //(E)
        //                    return expresion(root.ChildNodes.ElementAt(1));
        //            }

        //    }
        //    return "";
        //}

    }
}
