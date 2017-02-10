using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;
using System.Diagnostics;
using System.Collections;

namespace SBScript
{
    class PrimerRecorrido
    {
        public static String mensajeAux = "";
        public static Boolean concatenar = false;
        //
        public static Stack pilaAmbito = new Stack();
        
        public static String action(ParseTreeNode node)
        {
            String result = null;
            String variables = "";
            switch (node.Term.Name.ToString())
            {
                case "ENTRADA":
                    {
                       // MessageBox.Show("entrada");
                        if (node.ChildNodes.Count == 1)
                        {
                            pilaAmbito.Push("Global");
                            action(node.ChildNodes[0]);
                        }
                        break;
                    }
                case "ENCABEZADO":
                    {
                        //MessageBox.Show("encabezado");
                        if (node.ChildNodes.Count == 1)
                        {
                            action(node.ChildNodes[0]);
                        }
                        else if (node.ChildNodes.Count == 2)
                        {
                            action(node.ChildNodes[0]);
                            action(node.ChildNodes[1]);
                        }
                        break;
                    }
                case "C":
                    MessageBox.Show("c");
                    if (node.ChildNodes.Count == 2)
                    {
                        //action(node.ChildNodes[0]);
                    }
                    else if (node.ChildNodes.Count == 3)
                    {
                        //action(node.ChildNodes[0]);
                    }
                    break;

                case "TIPO":
                    {
                        // MessageBox.Show("DATA");
                        if (node.ChildNodes.Count == 1)
                        {
                            String[] numero = (node.ChildNodes.ElementAt(0).ToString().Split(' '));
                            result = numero[0];
                            MessageBox.Show(result);
                        }
                        break;
                    }
                case "DECLARACION":
                    {
                        //   MessageBox.Show("AtributoDATA");
                        if (node.ChildNodes.Count == 3)
                        {
                            action(node.ChildNodes[0]);
                            action(node.ChildNodes[1]);
                        }
                        else if (node.ChildNodes.Count == 5)
                        {
                            action(node.ChildNodes[0]);
                            action(node.ChildNodes[3]);
                        }
                        break;
                    }
                case "VAR":
                    {
                        if (node.ChildNodes.Count == 1)
                        {
                            String[] numero = (node.ChildNodes.ElementAt(0).ToString().Split(' '));
                            variables += numero[0] + ",";
                            result = variables;
                        }
                        else if (node.ChildNodes.Count == 3)
                        {
                            result = action(node.ChildNodes[0]);
                            String[] dato = node.ChildNodes.ElementAt(2).ToString().Split(' ');
                            result += dato[0] + ",";
                        }
                        break;
                    }
                case "ASIGNACION":
                    {
                        if (node.ChildNodes.Count == 3)
                        {
                            action(node.ChildNodes[2]);
                        }
                        else if (node.ChildNodes.Count == 4)
                        {
                            action(node.ChildNodes[2]);
                        }
                        break;
                    }
                case "MOSTRAR":
                    {
                        if (node.ChildNodes.Count == 5)
                        {
                            action(node.ChildNodes[2]);
                        }
                        break;
                    }
                case "DatosIMPRIMIR":
                    {

                        if (node.ChildNodes.Count == 1)
                        {
                            action(node.ChildNodes[0]);
                        }
                        else if (node.ChildNodes.Count == 3)
                        {
                            action(node.ChildNodes[0]);
                            action(node.ChildNodes[2]);
                        }

                        // MessageBox.Show("aITEM");
                        /*    if (node.ChildNodes.Count == 1)
                            {
                                action(node.ChildNodes[0]);
                            }
                            else if (node.ChildNodes.Count == 4)
                            {
                                String[] dato = (node.ChildNodes.ElementAt(0).ToString().Split(' '));
                                String id1 = dato[0];
                                String[] dato2 = (node.ChildNodes.ElementAt(2).ToString().Split(' '));
                                String id2 = dato2[0];
                                Listas.igualarVariables(id1, id2);
                            }
                            else if (node.ChildNodes.Count == 5)
                            {
                                String[] dato = (node.ChildNodes.ElementAt(0).ToString().Split(' '));
                                String id = dato[0];
                                String atributo = action(node.ChildNodes[2]);
                                String asignacion = action(node.ChildNodes[4]);

                                Boolean existe = Listas.asignarValor(id, atributo, asignacion);
                                if (existe == false)
                                {
                                    Listas.agregarError(dato[0], "La variable no existe.", "Error sintacticco");
                                }

                            }
                            else if (node.ChildNodes.Count == 8)
                            {
                                String tipo1 = action(node.ChildNodes[0]);
                                String tipo2 = action(node.ChildNodes[4]);



                            }*/
                        break;
                    }

                case "DIBUJAR":
                    {
                        // MessageBox.Show("OpcionITEM");
                        if (node.ChildNodes.Count == 5)
                        {
                            //result = action(node.ChildNodes[0]);
                        }
                        break;
                    }
                case "INSTRUCCIONES":
                    {
                        //  MessageBox.Show("ATRIBUTO");
                        if (node.ChildNodes.Count == 1)
                        {
                            action(node.ChildNodes[0]);
                        }
                        else if (node.ChildNodes.Count == 2)
                        {
                            action(node.ChildNodes[0]);
                            action(node.ChildNodes[1]);
                        }
                        break;
                    }
                case "INSTRUCCION":
                    {
                        //MessageBox.Show("TipoITEM");
                        if (node.ChildNodes.Count == 1)
                        {
                            action(node.ChildNodes[0]);
                        }
                        else if (node.ChildNodes.Count == 2)
                        {
                            //action(node.ChildNodes[0]);
                        }
                        break;
                    }
                case "METODO":
                    {
                        if (node.ChildNodes.Count == 6)
                        {
                         //   action(node.ChildNodes[2]);
                        }
                        else if (node.ChildNodes.Count == 7)
                        {
                            action(node.ChildNodes[5]);
                        }
                        else if(node.ChildNodes.Count == 8)
                        {
                            action(node.ChildNodes[3]);
                            action(node.ChildNodes[6]);
                        }
                        break;
                    }
                case "FUNCION":
                    {
                        if (node.ChildNodes.Count == 6)
                        {
                         //   action(node.ChildNodes[0]);
                        }
                        else if (node.ChildNodes.Count == 7)
                        {
                            //action(node.ChildNodes[0]);
                            //action(node.ChildNodes[1]);
                        }
                        else if (node.ChildNodes.Count == 8)
                        {
                            action(node.ChildNodes[3]);
                            action(node.ChildNodes[6]);
                        }

                        break;
                    }
                case "varPARAMETRO":
                    {
                        if (node.ChildNodes.Count == 2) //
                        {
                            action(node.ChildNodes[0]);
                        }
                        else if (node.ChildNodes.Count == 4) 
                        {
                            action(node.ChildNodes[0]);
                            action(node.ChildNodes[2]);
                        }


                     /*   variables = "";
                        //  MessageBox.Show("aCONFIG");
                        if (node.ChildNodes.Count == 1) //
                        {
                            action(node.ChildNodes[0]);
                        }
                        else if (node.ChildNodes.Count == 2) //Asignacion de variables
                        {
                            String vars = action(node.ChildNodes[0]);
                            String[] var = (vars.Split(','));
                            // action(node.ChildNodes[1]);
                            String asig = action(node.ChildNodes[1]);

                            for (int i = 0; i < var.Length - 1; i++)
                            {
                                Boolean existe = false;
                                for (int j = 0; j < Listas.listaVariables.Count; j++)
                                {
                                    Variable v = (Variable)Listas.listaVariables[j];
                                    String varNombre = v.nombre + " ";
                                    if (v.nombre == var[i] || varNombre == var[i])
                                    {
                                        existe = true;
                                        v.valor = asig;
                                    }
                                }
                                if (existe == false)
                                {
                                    MessageBox.Show("ER");
                                }

                            }
                        }
                        else if (node.ChildNodes.Count == 3) //Creacion de variables
                        {
                            String tipo = action(node.ChildNodes[0]);
                            String vars = action(node.ChildNodes[1]);
                            String[] var = (vars.Split(','));
                            String asig = action(node.ChildNodes[2]);

                            for (int i = 0; i < var.Length - 1; i++)
                            {
                                Variable v = new Variable();
                                v.tipo = tipo;
                                v.nombre = var[i];
                                v.valor = asig;
                                Listas.listaVariables.Add(v);
                            }
                                                   }*/
                        break;
                    }
                case "RETORNAR":
                    {
                        // MessageBox.Show("TIPO");
                        if (node.ChildNodes.Count == 3)
                        {
                            action(node.ChildNodes[1]);
                        }
                        break;
                    }
                case "MAIN":
                    {
                        //  MessageBox.Show("VAR");
                        if (node.ChildNodes.Count == 6)
                        {
                            action(node.ChildNodes[4]);
                        }
                        break;
                    }

                case "LLAMADA":
                    {
                        //MessageBox.Show("ASIGNACION");
                        if (node.ChildNodes.Count == 4)
                        {
                            action(node.ChildNodes[2]);
                        }
                        else if (node.ChildNodes.Count == 5)
                        {
                            result = action(node.ChildNodes[2]);
                        }
                        break;
                    }
                case "TipoPARAMETRO":
                    {
                        //MessageBox.Show("IMPRIMIR");
                        if (node.ChildNodes.Count == 3)
                        {

                        }
                        break;
                    }
                case "SI":
                    {
                        // MessageBox.Show("DatosIMPRIMIR");
                        if (node.ChildNodes.Count == 7)
                        {
                            action(node.ChildNodes[2]);
                            action(node.ChildNodes[5]);
                        }
                        else if (node.ChildNodes.Count == 8)
                        {
                            action(node.ChildNodes[2]);
                            action(node.ChildNodes[5]);
                            action(node.ChildNodes[7]); 
                        }
                        break;
                    }
                case "ELSE":
                    {
                        //MessageBox.Show("POSICION");
                        if (node.ChildNodes.Count == 2)
                        {
                            action(node.ChildNodes[1]);
                        }
                        else if (node.ChildNodes.Count == 4)
                        {
                            action(node.ChildNodes[2]);
                        }
                        break;
                    }
                case "CICLO":
                    {
                        if (node.ChildNodes.Count == 7)
                        {
                            action(node.ChildNodes[2]);
                            action(node.ChildNodes[5]);
                        }
                        break;
                    }
                case "SWITCH":
                    {
                        if (node.ChildNodes.Count == 5)
                        {
                            action(node.ChildNodes[2]);
                            action(node.ChildNodes[4]);
                        }
                        else if (node.ChildNodes.Count == 6)
                        {
                            action(node.ChildNodes[2]);
                            action(node.ChildNodes[4]);
                            action(node.ChildNodes[5]);
                        }
                        else if (node.ChildNodes.Count == 7)
                        {
                            action(node.ChildNodes[2]);
                            action(node.ChildNodes[5]);
                        }
                        break;
                    }
                case "DEFAULT":
                    {
                        if (node.ChildNodes.Count == 5)
                        {
                            action(node.ChildNodes[3]);
                        }
                        break;
                    }
                case "FOR":
                    {
                        if (node.ChildNodes.Count == 7)
                        {
                            action(node.ChildNodes[2]);
                            action(node.ChildNodes[5]);
                        }
                        break;
                    }
                case "PARA":
                    {
                        if (node.ChildNodes.Count == 8)
                        {
                            action(node.ChildNodes[3]);
                            action(node.ChildNodes[5]);
                            action(node.ChildNodes[7]);
                        }
                        break;
                    }
                case "OP":
                    {
                        if (node.ChildNodes.Count == 1)
                        {
                            result = action(node.ChildNodes[0]);
                        }
                        break;
                    }
                case "OpARITMETICO":
                    {
                        if (node.ChildNodes.Count == 1)
                        {
                            result = action(node.ChildNodes[0]);
                        }
                        break;
                    }
                case "E":
                    {
                        //    MessageBox.Show("E");
                        if (node.ChildNodes.Count == 1)
                        {
                            result = resolverOpAritmetica(node).ToString();
                        }
                        else if (node.ChildNodes.Count == 3)
                        {
                            result = resolverOpAritmetica(node).ToString();

                        }
                        break;
                    }
                case "OpRELACIONAL":
                    {
                        if (node.ChildNodes.Count == 1)
                        {
                            result = action(node.ChildNodes[0]);
                        }
                        break;
                    }
                case "R":
                    {
                        // MessageBox.Show("R");
                        if (node.ChildNodes.Count == 1)
                        {
                            result = resolverOpRelacional(node).ToString();
                        }
                        else if (node.ChildNodes.Count == 3)
                        {
                            result = resolverOpRelacional(node).ToString();
                        }
                        break;
                    }
                case "OpLOGICO":
                    {
                        //  MessageBox.Show("Logico");
                        if (node.ChildNodes.Count == 1)
                        {
                            result = action(node.ChildNodes[0]);
                        }
                        break;
                    }
                case "L":
                    {
                        //MessageBox.Show("L");
                        if (node.ChildNodes.Count == 1)
                        {
                            result = resolverOpLogico(node).ToString();
                        }
                        else if (node.ChildNodes.Count == 2)
                        {
                            result = resolverOpLogico(node).ToString();

                        }
                        else if (node.ChildNodes.Count == 3)
                        {
                            result = resolverOpLogico(node).ToString();

                        }
                        break;
                    }
                default:
                    break;
            }
            return result;


        }

        public static String resolverOpAritmetica(ParseTreeNode root)
        {
            concatenar = false;
            String resultado = expresion(root);
            //     MessageBox.Show("El resultado es: " + resultado);
            return resultado;
        }

        private static String expresion(ParseTreeNode root)
        {
            switch (root.ChildNodes.Count)
            {
                case 1:
                    String[] dato = (root.ChildNodes.ElementAt(0).ToString().Split('('));
                    //
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
                       /*Variable v = Listas.obtenerVariable(dato[0]);
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
                        }*/
                        return "";
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
