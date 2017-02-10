using System;
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

    class MF
    {

        public String nombre;
        public String tipo;
        public String retorno;
        public String nodo;
        public ArrayList parametro;
    }

    class Parametro
    {
        public String nombre;
        public String tipo;

    }

}
