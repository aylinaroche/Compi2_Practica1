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
        public static double incerteza = 0.01;
        public static string ruta = "";
        public static ArrayList MensajeConsola = new ArrayList();


        public static Boolean compararCadenas(String cadena1, String cadena2)
        {
            // igual_mayor = true;
            int aumenta = cadena2.Length;
            if (cadena1.Length <= cadena2.Length)
            {
                aumenta = cadena1.Length;
            }

            for (int i = 0; i < aumenta; i++)
            {
                string c1 = cadena1.Substring(i, 1);
                int ascii1 = Encoding.ASCII.GetBytes(c1)[0];
                string c2 = cadena2.Substring(i, 1);
                int ascii2 = Encoding.ASCII.GetBytes(c2)[0];

                if (ascii1 > ascii2)
                {
                    return true;
                }
                else if (ascii1 < ascii2)
                {
                    return false;
                }
                
            }

            if (cadena1.Length >= cadena2.Length)
            {
                return true;
            }else
            {
                return false;
            }
        }

        public static Boolean semejarCadenas(String cadena1, String cadena2)
        {
            // igual_mayor = true;
            String nueva1 = quitarEspaciosFinal(quitarEspaciosInicio(cadena1));
            String nueva2 = quitarEspaciosFinal(quitarEspaciosInicio(cadena2));

            int i = 0;
            int aumenta = nueva2.Length;
            if (nueva1.Length <= nueva2.Length)
            {
                aumenta = nueva1.Length;
            }

            for ( i = 0; i < aumenta; i++)
            {
                string c1 = nueva1.Substring(i, 1);
                int ascii1 = Encoding.ASCII.GetBytes(c1)[0];
                string c2 = nueva2.Substring(i, 1);
                int ascii2 = Encoding.ASCII.GetBytes(c2)[0];

                if (ascii1 > ascii2)
                {
                    return false;
                }
                else if (ascii1 < ascii2)
                {
                    return false;
                }

            }

            return true;
        }

        public static String quitarEspaciosInicio(String cadena)
        {
            int i = 0;
            String cadenaNueva = "";
            ArrayList caracter = new ArrayList();
            Boolean espacio = true;
            for (i = 0; i < cadena.Length; i++)
            {
                string c1 = cadena.Substring(i, 1);

                if (espacio == true)
                {
                    if (c1 != " ")
                    {
                        cadenaNueva += c1;
                        espacio = false;
                    }
                }else
                {
                    cadenaNueva += c1;
                }
            }

            return cadenaNueva;
        }

        public static String quitarEspaciosFinal(String cadena)
        {
            int i = 0; 
            String cadenaNueva = "";
            ArrayList caracter = new ArrayList();
            Boolean espacio = true;
            for (i = cadena.Length-1;i>-1 ; i--)
            {
                string c1 = cadena.Substring(i, 1);

                if (espacio == true)
                {
                    if (c1 != " ")
                    {
                        cadenaNueva += c1;
                        espacio = false;
                    }
                }
                else
                {
                    cadenaNueva += c1;
                }
            }

            String inversa = "";

            for (i = cadenaNueva.Length - 1; i > -1; i--)
            {
                string c1 = cadenaNueva.Substring(i, 1);
                inversa += c1;
            }
            return inversa;
        }
    }

}
