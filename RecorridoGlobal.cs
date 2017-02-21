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
                    Listas.nodoActual = root.ChildNodes[0];
                    if (dato[1] == "cadena)")
                    {
                        PrimerRecorrido.concatenar = true;
                        String cadena = dato[0].Substring(0, dato[0].Length - 1);
                        Listas.expresion += cadena;
                        return cadena;
                    }
                    else if (dato[1] == "numero)")
                    {
                        String n = Listas.quitarEspaciosFinal(dato[0]);
                        PrimerRecorrido.concatenar = false;
                        Listas.expresion += n;
                        return n;
                    }
                    else if (dato[1] == "id)")
                    {
                        Listas.expresion += dato[0];
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
                            Reporte.agregarMensajeError("El id '" + dato[0] + "' no existe", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                            // MessageBox.Show("ERROR"); //No encuentra el id
                            PrimerRecorrido.concatenar = true;
                            return "";
                        }
                        //   return "";
                    }
                    else if (dato[1] == "Keyword)")
                    {
                        Listas.expresion += dato[0];
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
                        Listas.expresion += dato[0];
                        String r = action(root.ChildNodes[0]);
                        retornar = false;
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
                            boolean = false;
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            Boolean c2 = PrimerRecorrido.concatenar;
                            Boolean b2 = boolean;
                            if (b1 == false || b2 == false)
                            {
                                if (c1 == true || c2 == true)
                                {
                                    if (E1 == "" || E2 == "")
                                    {
                                        Reporte.agregarMensajeError("No se ha inicializado la variable", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                                        return "";
                                    }
                                    else
                                    {
                                        resultado = E1 + E2;
                                        PrimerRecorrido.concatenar = true;
                                        return resultado;
                                    }
                                }
                                else
                                {
                                    if (E1 == "" || E2 == "")
                                    {
                                        Reporte.agregarMensajeError("No se ha inicializado la variable", "Error Semantico", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                                        return "";
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
                                    Reporte.agregarMensajeError("Error al dividir con '0'", "Error Ejecucion", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
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
                                if (resultado == "NaN" || resultado == "∞")
                                {
                                    Reporte.agregarMensajeError("Error al operar modulo con '0'", "Error Ejecucion", Listas.nodoActual.Token.Location.Line, Listas.nodoActual.Token.Location.Column);
                                    return "0";
                                }
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
                                    PrimerRecorrido.concatenar = false;
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
                                    PrimerRecorrido.concatenar = false;
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
                                    PrimerRecorrido.concatenar = false;
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
                                    PrimerRecorrido.concatenar = false;
                                    if (Listas.compararCadenasMenorIgual(E1, E2))
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
                                    PrimerRecorrido.concatenar = false;
                                    if (Listas.compararCadenasDiferente(E1, E2))
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
                                    PrimerRecorrido.concatenar = false;
                                    if (Listas.compararCadenasDiferente(E1, E2))
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
                                    PrimerRecorrido.concatenar = false;
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
                        case "!&":
                            E1 = expresion(root.ChildNodes.ElementAt(0));
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            try
                            {
                                if (E1 == E2)
                                {
                                    return "0";
                                }
                                else
                                {
                                    return "1";
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
