using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBScript
{
    class Reporte
    {
        public static ArrayList errores = new ArrayList();

        public static void agregarError(String nombre, String tipo, int fila, int columna)
        {
            errores.Add(new Error(nombre, tipo, fila, columna));
        }


        public static void agregarMensajeError(String nombre, String tipo, int fila, int columna)
        {
            errores.Add(new Error(nombre, tipo, fila, columna));
            Listas.MensajeConsola.Add("> "+nombre+".\n");
        }


        public static void generarReporte()
        {
            StringWriter salida = new StringWriter();
            try
            {
                salida.WriteLine("<html>");
                salida.WriteLine("<head><title>SBScript</title></head>");
                salida.WriteLine("<body bgcolor=\"black\">");
                salida.WriteLine("<h1><center><FONT COLOR=silver>PRACTICA 1<FONT></center></h1>\"");
                salida.WriteLine("<h1><center><FONT COLOR=81426E> " + "SBScript" + " <FONT></center></h1>");
                salida.WriteLine("<br>");
                salida.WriteLine("<center>");
                salida.WriteLine("<table border= 1 width= 500>");
                salida.WriteLine("<tr>");
                salida.WriteLine("<th><font color=\"#24AAFF\" face=\"courier new\"> TIPO </font></th>");
                salida.WriteLine("<th><font color=\"#24AAFF\" face=\"courier new\"> DESCRIPCION </font></th>");
                salida.WriteLine("<th><font color=\"#24AAFF\" face=\"courier new\"> ARCHIVO </font></th>");
                salida.WriteLine("<th><font color=\"#24AAFF\" face=\"courier new\"> FILA </font></th>");
                salida.WriteLine("<th><font color=\"#24AAFF\" face=\"courier new\"> COLUMNA </font></th>");
                salida.WriteLine("</tr>");

                for (int i = 0; i < errores.Count; i++)
                {
                    Error s = (Error)errores[i];
                    salida.WriteLine("<tr>");
                    salida.WriteLine("<th><font color=\"white\">" + s.tipo + "</font></th>");
                    salida.WriteLine("<th><font color=\"white\">" + s.nombre + "</font></th>");
                    salida.WriteLine("<th><font color=\"white\">" + s.tipo + "</font></th>");
                    salida.WriteLine("<th><font color=\"white\">" + s.fila + "</font></th>");
                    salida.WriteLine("<th><font color=\"white\">" + s.columna + "</font></th>");
                }
                salida.WriteLine("</table><br>");
                salida.WriteLine("</body></html>");
                Listas.MensajeConsola.Add("> Reporte creado correctamente.\n");
            }
            catch (IOException e)
            {
                Listas.MensajeConsola.Add("> Error al crear el reporte: "+"\n");
            }
            if (Listas.ruta == "")
            {
                Listas.ruta="C:/Users/Aylin/Documents/Visual Studio 2015/Projects/SBScript";
            }
            String ruta = Listas.ruta+"/Reporte.html";
            
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(ruta))
            {  
                file.WriteLine(salida);
            }
            try
            {
                Process.Start(ruta);
            }
            catch (Exception e)
            {
               // MessageBox.Show(e.ToString());
            }

        }

    }
    class Error
    {
        public String nombre;
        public String tipo;
        public int fila;
        public int columna;
        public Error(String nombre, String tipo, int fila, int columna)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.fila = fila;
            this.columna = columna;
        }


    }
}
