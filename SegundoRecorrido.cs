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
    class SegundoRecorrido
    {
        public static String mensajeAux = "";
        public static Boolean concatenar = false;
        //
        public static Stack pilaAmbito = new Stack();
        public static int contador = 0;
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
                            String ambito = pilaAmbito.Peek().ToString();
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
                            String ambito = pilaAmbito.Peek().ToString();
                            if (asig != "error")
                            {
                                for (int i = 0; i < var.Length - 1; i++)
                                {
                                    Variables.crearVariable(tipo, var[i], asig, ambito);
                                }
                            }
                            else
                            {
                                Reporte.agregarMensajeError("No puede declarar una expresion en una variable global", "Error Semantico", 0, 0);
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
                            String id = action(nodo.ChildNodes[0]);
                            String asig = action(nodo.ChildNodes[2]);
                            Variable v = new Variable();
                            Reporte.agregarMensajeError("No puede asignar una expresion en una variable global", "Error Semantico", 0, 0);
                        }
                        break;
                    }
                case "MOSTRAR":
                    {
                        contador = 0;
                        if (nodo.ChildNodes.Count == 5)
                        {
                           result=action(nodo.ChildNodes[2]);
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
                            String expr = action(nodo.ChildNodes[0]);
                            try
                            {
                                cadena.Replace(indice, expr);
                            }catch(Exception ex)
                            {
                                MessageBox.Show("EE");
                            }
                            result = cadena;
                        }
                        contador += 1;
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
                        if (nodo.ChildNodes.Count == 6)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            Metodo_Funcion.agregarMF("Void", dato[0], "", null, null);
                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            Metodo_Funcion.agregarMF("Void", dato[0], "", nodo.ChildNodes[5], null);
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            action(nodo.ChildNodes[3]);
                            String[] dato = (nodo.ChildNodes.ElementAt(0).ToString().Split(' '));
                            Metodo_Funcion.agregarMF("Void", dato[0], "", nodo.ChildNodes[6], Metodo_Funcion.parametros);
                        }
                        break;
                    }
                case "FUNCION":
                    {
                        Metodo_Funcion.parametros.Clear();
                        if (nodo.ChildNodes.Count == 6)
                        {
                            String tipo = action(nodo.ChildNodes[0]);
                            String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                            Metodo_Funcion.agregarMF(tipo, dato[0], "", null, null);
                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            String tipo = action(nodo.ChildNodes[0]);
                            String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                            if (nodo.ChildNodes[3].Term.Name.ToString() == "varPARAMETRO")
                            {
                                action(nodo.ChildNodes[3]);
                                Metodo_Funcion.agregarMF(tipo, dato[0], "", null, Metodo_Funcion.parametros);
                            }
                            else
                            {
                                Metodo_Funcion.agregarMF(tipo, dato[0], "", nodo.ChildNodes[5], null);
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
                        if (nodo.ChildNodes.Count == 6)
                        {
                            Metodo_Funcion.agregarMF("MAIN", "MAIN", "", nodo.ChildNodes[4], null);
                        }
                        break;
                    }

                case "LLAMADA":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                            ParseTreeNode metodo = Metodo_Funcion.buscarMetodo(dato[0]);
                            if (metodo != null)
                            {
                                pilaAmbito.Push(dato[0]);
                                action(metodo);
                                pilaAmbito.Pop();
                            }else
                            {
                                Reporte.agregarMensajeError("El metodo/funcion no existe", "Error Semantico", 0, 0);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            String[] dato = (nodo.ChildNodes.ElementAt(1).ToString().Split(' '));
                            ParseTreeNode metodo = Metodo_Funcion.buscarMetodo(dato[0]);
                            if (metodo != null)
                            {
                                pilaAmbito.Push(dato[0]);
                                action(metodo);
                                pilaAmbito.Pop();
                            }
                            else
                            {
                                Reporte.agregarMensajeError("El metodo/funcion no existe", "Error Semantico", 0, 0);
                            }
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
            concatenar = false;
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
                        concatenar = true;
                        return dato[0];
                    }
                    else if (dato[1] == "numero)")
                    {
                        concatenar = false;
                        return dato[0];
                    }
                    else if (dato[1] == "id)")
                    {
                        Variable v = Variables.obtenerVariable(dato[0]);
                        if (v != null)
                        {
                            if (v.tipo == "string")
                            {
                                concatenar = true;
                            }
                            return v.valor;
                        }
                        else
                        {
                            MessageBox.Show("ERROR"); //No encuentra el id
                            concatenar = true;
                            return "";
                        }
                        //   return "";
                    }
                    else if (dato[1] == "Keyword)")
                    {
                        concatenar = false;
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
                        return "error";
                    }
                    else
                    {
                        concatenar = true;
                        return "";
                    }


                case 3: //Nodo binario
                    String resultado = "";
                    String E1 = "";
                    String E2 = "";


                    switch (root.ChildNodes.ElementAt(1).ToString().Substring(0, 1))
                    {

                        case "+": //E+E
                            E1 = expresion(root.ChildNodes.ElementAt(0));
                            Boolean c1 = concatenar;
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            Boolean c2 = concatenar;
                            if (c1 == true || c2 == true)
                            {
                                resultado = E1 + E2;
                                concatenar = true;
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
                                    concatenar = false;
                                    return resultado;

                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("E");
                                    return "";
                                }

                            }

                        case "-": //E-E
                            E1 = expresion(root.ChildNodes.ElementAt(0));
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            try
                            {
                                Double r = Convert.ToDouble(E1) - Convert.ToDouble(E2); ;
                                resultado = r.ToString();
                                return resultado;

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("E");
                                return "";
                            }
                        case "/": //E/E
                            E1 = expresion(root.ChildNodes.ElementAt(0));
                            E2 = expresion(root.ChildNodes.ElementAt(2));
                            try
                            {
                                Double r = Convert.ToDouble(E1) / Convert.ToDouble(E2); ;
                                resultado = r.ToString();
                                return resultado;

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("E");
                                return "";
                            }
                        case "*": //E*E
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
                        case ".":
                            // Listas.concatenar = false;
                            concatenar = false;
                            //MessageBox.Show("punto");
                            String[] datos = root.ChildNodes.ElementAt(0).ToString().Split(' ');
                            String id = datos[0];
                            String atributo = action(root.ChildNodes[2]);
                            /*resultado = Listas.obtenerValor(id, atributo);

                            if (Listas.concatenar == true)
                            {
                                concatenar = true;
                            }
                            */
                            return resultado;


                        default: //(E)
                            return expresion(root.ChildNodes.ElementAt(1));
                    }

            }
            return "";
        }

        public static String resolverOpRelacional(ParseTreeNode root)

        {
            String resultado = expresionR(root);
            //   MessageBox.Show("El resultado es: " + resultado);
            return resultado;
        }

        private static String expresionR(ParseTreeNode root)
        {
            switch (root.ChildNodes.Count)
            {
                case 1: //Nodo hoja
                        //   MessageBox.Show("caso 1");
                    String[] dato = (root.ChildNodes.ElementAt(0).ToString().Split(' '));

                    if (dato[1] == "(numero)")
                    {
                        MessageBox.Show("entro en numero");
                        try
                        {
                            return (dato[0].ToString());
                        }
                        catch (Exception e)
                        {
                            mensajeAux = "error::: " + dato[0].ToString();
                            return "false";
                        }
                    }
                    else if (dato[1] == "(id)")
                    {
                        MessageBox.Show("entro en id");
                        return (dato[0].ToString());
                        //buscar variable en lista de variables
                    }
                    else if (dato[1] == "(Keyword)")
                    {
                        return (dato[0].ToString());
                    }
                    else if (dato[1] == "STRING")
                    {
                        return (dato[0].ToString());
                    }
                    else
                    {
                        MessageBox.Show("entro en else");
                        return "false";
                    }


                case 3: //Nodo binario
                        //    MessageBox.Show("caso 3");
                    String[] simbolo = (root.ChildNodes.ElementAt(0).ToString().Split(' '));
                    MessageBox.Show("simbolo=" + root.ChildNodes.ElementAt(1).ToString().Substring(0, 2));
                    Double var1, var2;
                    switch (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2))
                    {

                        case "> ":
                            try
                            {
                                var1 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(0)));
                                var2 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(2)));

                                if (var1 > var2)
                                {
                                    return "true";
                                }
                                else
                                {
                                    return "false";
                                }
                            }
                            catch
                            {
                                return "error";
                            }



                        case "< ":
                            try
                            {
                                var1 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(0)));
                                var2 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(2)));

                                if (var1 < var2)
                                {
                                    return "true";
                                }
                                else
                                {
                                    return "false";
                                }
                            }
                            catch
                            {
                                return "error";
                            }
                        case ">=":
                            try
                            {
                                var1 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(0)));
                                var2 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(2)));

                                if (var1 >= var2)
                                {
                                    return "true";
                                }
                                else
                                {
                                    return "false";
                                }
                            }
                            catch
                            {
                                return "error";
                            }
                        case "<=":
                            try
                            {
                                var1 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(0)));
                                var2 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(2)));

                                if (var1 <= var2)
                                {
                                    return "true";
                                }
                                else
                                {
                                    return "false";
                                }
                            }
                            catch
                            {
                                return "error";
                            }
                        case "==":
                            try
                            {
                                var1 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(0)));
                                var2 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(2)));

                                if (var1 == var2)
                                {
                                    return "true";
                                }
                                else
                                {
                                    return "false";
                                }
                            }
                            catch
                            {
                                return "error";
                            }
                        case "!=":
                            try
                            {
                                var1 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(0)));
                                var2 = Convert.ToDouble(expresion(root.ChildNodes.ElementAt(2)));

                                if (var1 != var2)
                                {
                                    return "true";
                                }
                                else
                                {
                                    return "false";
                                }
                            }
                            catch
                            {
                                return "error";
                            }
                        default:
                            //return expresion(root.ChildNodes.ElementAt(1));
                            return "error D";
                    }

            }
            return "null";
        }

        public static String resolverOpLogico(ParseTreeNode root)

        {
            String resultado = expresionL(root);
            //           MessageBox.Show("El resultado es: " + resultado);
            return resultado;
        }

        private static String expresionL(ParseTreeNode root)
        {
            switch (root.ChildNodes.Count)
            {
                case 1: //Nodo hoja

                    String result = action(root.ChildNodes[0]);
                    return result;
                case 2:

                    String var = action(root.ChildNodes[1]);
                    if (var == "false")
                    {
                        return "true";
                    }
                    else
                    {
                        return "false";
                    }

                case 3: //Nodo binario

                    String var1 = action(root.ChildNodes[0]);
                    String var2 = action(root.ChildNodes[2]);


                    String[] simbolo = (root.ChildNodes.ElementAt(0).ToString().Split(' '));
                    //                 MessageBox.Show("simbolo=" + root.ChildNodes.ElementAt(1).ToString().Substring(0, 3));

                    switch (root.ChildNodes.ElementAt(1).ToString().Substring(0, 3))
                    {

                        case "&& ":
                            if (var1 == "true" && var2 == "true" || var1 == "false" && var2 == "false")
                            {
                                return "true";
                            }
                            else
                            {
                                return "false";
                            }

                        case "|| ":
                            if (var1 == "true" || var2 == "true")
                            {
                                return "true";
                            }
                            else
                            {
                                return "false";
                            }
                        default:
                            //return expresion(root.ChildNodes.ElementAt(1));
                            return "E";
                    }

            }
            return "null";
        }

        public static String obtenerImprimir(ParseTreeNode root)
        {
            MessageBox.Show("obtener I =" + root.ToString() + ", " + root.ChildNodes.Count);
            String resultado = datosImprimir(root);
            MessageBox.Show("El resultado es: " + resultado);
            return resultado;
        }

        private static String datosImprimir(ParseTreeNode root)
        {
            switch (root.ChildNodes.Count)
            {
                case 1: //Nodo hoja
                    try
                    {
                        String[] dato = (root.ChildNodes.ElementAt(0).ToString().Split('('));
                        MessageBox.Show("dato = " + (dato[1].ToString()));
                        if (dato[1] == "numero)")
                        {
                            MessageBox.Show("entro en numero");
                            return (dato[0].ToString());
                        }
                        else if (dato[1] == "id)")
                        {
                            MessageBox.Show("entro en id");
                            String valor = "";// = obtenerValorVariable(dato[0].ToString());
                            return valor;

                        }
                        else if (dato[1] == "Keyword)")
                        {
                            MessageBox.Show("entro en key");
                            return (dato[0].ToString());
                        }
                        else if (dato[1] == "STRING)")
                        {
                            MessageBox.Show("entro en cadena");
                            return (dato[0].ToString());
                        }
                        else
                        {
                            MessageBox.Show("entro en else");
                            String result = action(root.ChildNodes[0]);
                            return "false";
                        }
                    }
                    catch
                    {
                        MessageBox.Show("CATCH");
                        String result = action(root.ChildNodes[0]);
                        MessageBox.Show("Result del catch " + result);
                        return result;
                    }


                case 3: //Nodo binario
                    String var1 = datosImprimir(root.ChildNodes[0]);
                    String var2 = datosImprimir(root.ChildNodes[2]);


                    String[] simbolo = (root.ChildNodes.ElementAt(0).ToString().Split(' '));
                    MessageBox.Show("simbolo=" + root.ChildNodes.ElementAt(1).ToString().Substring(0, 1));

                    switch (root.ChildNodes.ElementAt(1).ToString().Substring(0, 1))
                    {

                        case "+":
                            String texto = "";
                            texto = var1 + var2;
                            return texto;

                        default:

                            return "error D";
                    }

            }
            return "vacio";
        }

    }
}
