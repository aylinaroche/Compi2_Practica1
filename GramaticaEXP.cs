using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace SBScript
{
    class GramaticaEXP : Grammar
    {
        public GramaticaEXP() : base(caseSensitive: true)
        {
            #region ERs
            RegexBasedTerminal letra = new RegexBasedTerminal("letra", "[0-9]*[a-zA-Z][0-9a-zA-Z]");
            NumberLiteral numero = new NumberLiteral("numero");
            IdentifierTerminal id = new IdentifierTerminal("id");
            StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.IsTemplate);

            #endregion

            #region Terminales
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var por = ToTerm("*");
            var div = ToTerm("/");
            var potencia = ToTerm("^");
            var modulo = ToTerm("%");
            var aumentar = ToTerm("++");
            var disminuir = ToTerm("--");
            var diferente = ToTerm("!=");
            var mayor = ToTerm(">");
            var menor = ToTerm("<");
            var menori = ToTerm("<=");
            var mayori = ToTerm(">=");
            var igual = ToTerm("=");
            var igualDoble = ToTerm("==");
            var or = ToTerm("||");
            var and = ToTerm("&&");
            var not = ToTerm("!");
            var xor = ToTerm("!&");
            var semejante = ToTerm("~");
            var coma = ToTerm(",");
            var puntoComa = ToTerm(";");
            var dosPuntos = ToTerm(":");
            var verdadero = ToTerm("true");
            var falso = ToTerm("false");
            var parentesisA = ToTerm("(");
            var parentesisC = ToTerm(")");
            var corcheteA = ToTerm("[");
            var corcheteC = ToTerm("]");
            var llaveA = ToTerm("{");
            var llaveC = ToTerm("}");
            var extension = ToTerm(".sbs");
            #endregion

            #region No Terminales
            NonTerminal
                ENTRADA = new NonTerminal("ENTRADA"),
                ENCABEZADO = new NonTerminal("ENCABEZADO"),
                INICIO = new NonTerminal("INICIO"),
                LLAMADA = new NonTerminal("LLAMADA"),
                TipoPARAMETRO = new NonTerminal("TipoPARAMETRO"),
                S = new NonTerminal("S"),
                L = new NonTerminal("L"),
                E = new NonTerminal("E");
            #endregion

            #region Gramatica
            ENTRADA.Rule = E; ///

            LLAMADA.Rule = id + parentesisA + TipoPARAMETRO + parentesisC + puntoComa
                | id + parentesisA + TipoPARAMETRO + parentesisC
                | id + parentesisA + parentesisC + puntoComa
                | id + parentesisA + parentesisC;

            TipoPARAMETRO.Rule = TipoPARAMETRO + coma + E
                | E;

            E.Rule = parentesisA + E + parentesisC
                | E + mas + E
                | E + menos + E
                | E + por + E
                | E + div + E
                | E + potencia + E
                | E + modulo + E
                | E + mayor + E
                | E + menor + E
                | E + mayori + E
                | E + menori + E
                | E + igualDoble + E
                | E + diferente + E
                | E + semejante + E
                | E + or + E
                | E + and + E
                | not + E
                | E + aumentar
                | E + disminuir
                | aumentar
                | disminuir
                | numero
                | id
                | cadena
                | falso
                | verdadero
                | LLAMADA
                | menos + parentesisA + E + parentesisC
                | menos + E;

            #endregion
            MarkTransient(TipoPARAMETRO);
            MarkPunctuation(parentesisA, parentesisC);

            //MarkTransient(LLAMADA);

            #region Preferencias
            this.Root = E;

            this.RegisterOperators(1, Associativity.Left, "==", "!=", "<", ">", "<=", ">=", "~");
            this.RegisterOperators(2, Associativity.Left, "+", "-");
            this.RegisterOperators(3, Associativity.Left, "*", "/", "%");
            this.RegisterOperators(4, Associativity.Right, "^");
            //this.RegisterOperators(4, Associativity.Neutral, "- ");
            //            this.RegisterOperators(1, Associativity.Left, "!=", "<", ">", "<=", ">=", "~");
            this.RegisterOperators(6, Associativity.Left, "||");
            this.RegisterOperators(7, Associativity.Left, "!&", "|&");
            this.RegisterOperators(8, Associativity.Left, "&&");
            this.RegisterOperators(9, Associativity.Left, "!");
            #endregion
        }
    }
}
