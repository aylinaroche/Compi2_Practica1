using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace SBScript
{
    public class Gramatica : Grammar
    {
        public Gramatica() : base(caseSensitive: true)
        {
            #region ERs
            RegexBasedTerminal letra = new RegexBasedTerminal("letra", "[0-9]*[a-zA-Z][0-9a-zA-Z]");
            //RegexBasedTerminal numero = new RegexBasedTerminal("numero", "[+|-]?[0-9]+");
            RegexBasedTerminal decim = new RegexBasedTerminal("decimal", "[0-9]* . [0-9]*");
            NumberLiteral numero = new NumberLiteral("numero");
            IdentifierTerminal id = new IdentifierTerminal("id");

            CommentTerminal comLine = new CommentTerminal("comLine", "#", "\n", "\r\n");
            CommentTerminal com = new CommentTerminal("com", "/n", "/r");
            CommentTerminal comMulti = new CommentTerminal("comMulti", "#*", "*#");
            CommentTerminal comM = new CommentTerminal("comM", "#*", "*#");
            StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.IsTemplate);

            base.NonGrammarTerminals.Add(com);
            base.NonGrammarTerminals.Add(comLine);
            base.NonGrammarTerminals.Add(comMulti);
            base.NonGrammarTerminals.Add(comM);
            #endregion

            #region Terminales
            var tipoString = ToTerm("String");
            var tipoNumber = ToTerm("Number");
            var retornar = ToTerm("Retorno");
            var tipoBoolean = ToTerm("Bool");
            var tipoVoid = ToTerm("Void");
            var incluye = ToTerm("Incluye");
            var define = ToTerm("Define");
            var mostrar = ToTerm("Mostrar");
            var principal = ToTerm("Principal");
            var si = ToTerm("Si");
            var sino = ToTerm("Sino");
            var sinosi = ToTerm("Sino_Si");
            var selecciona = ToTerm("Selecciona");
            var caso = ToTerm("caso");
            var defecto = ToTerm("Defecto");
            var para = ToTerm("Para");
            var hasta = ToTerm("Hasta");
            var mientras = ToTerm("Mientras");
            var continuar = ToTerm("Continuar");
            var detener = ToTerm("Detener");
            var dibujarAST = ToTerm("DibujarAST");
            var dibujarEXP = ToTerm("DibujarEXP");
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
                DECLARACION = new NonTerminal("DECLARACION"),
                FOR = new NonTerminal("FOR"),
                TIPO = new NonTerminal("TIPO"),
                METODO = new NonTerminal("METODO"),
                MOSTRAR = new NonTerminal("MOSTRAR"),
                VAR = new NonTerminal("VAR"),
                FUNCION = new NonTerminal("FUNCION"),
                DibujarAST = new NonTerminal("DibujarAST"),
                DibujarEXP = new NonTerminal("DibujarEXP"),
                OpLOGICO = new NonTerminal("OpLOGICO"),
                OpARITMETICO = new NonTerminal("OpARITMETICO"),
                OpRELACIONAL = new NonTerminal("OpRELACIONAL"),
                OP = new NonTerminal("OP"),
                CICLO = new NonTerminal("CICLO"),
                SI = new NonTerminal("SI"),
                SWITCH = new NonTerminal("SWITCH"),
                TipoPARAMETRO = new NonTerminal("TipoPARAMETRO"),
                varPARAMETRO = new NonTerminal("varPARAMETRO"),
                CASE = new NonTerminal("CASE"),
                ELSE = new NonTerminal("ELSE"),
                DEFAULT = new NonTerminal("DEFAULT"),
                INSTRUCCIONES = new NonTerminal("INSTRUCCIONES"),
                INSTRUCCION = new NonTerminal("INSTRUCCION"),
                LLAMADA = new NonTerminal("LLAMADA("),
                CONTINUAR = new NonTerminal("CONTINUAR"),
                MAIN = new NonTerminal("MAIN"),
                DatosMOSTRAR = new NonTerminal("DatosMOSTRAR"),
                RETORNAR = new NonTerminal("RETORNAR"),
                ASIGNACION = new NonTerminal("ASIGNACION"),
                PARA = new NonTerminal("PARA"),
                R = new NonTerminal("R"),
                S = new NonTerminal("S"),
                L = new NonTerminal("L"),
                E = new NonTerminal("E"),
                I = new NonTerminal("I"),
                C = new NonTerminal("C"),
                G = new NonTerminal("G");
            #endregion

            #region Gramatica
            ENTRADA.Rule = INICIO;

            INICIO.Rule = ENCABEZADO + INSTRUCCIONES
                | INSTRUCCIONES
                | ENCABEZADO; ///

            ENCABEZADO.Rule = ENCABEZADO + C
                | C;

            C.Rule = incluye + id + extension
                | define + numero
                | define + cadena;

            C.ErrorRule = SyntaxError + "}";

            TIPO.Rule = tipoNumber
                 | tipoString
                 | tipoBoolean;

            DECLARACION.Rule = TIPO + VAR + puntoComa
                | TIPO + VAR + igual + OP + puntoComa
                | TIPO + VAR + igual + OP;

            VAR.Rule = VAR + coma + id
                | id;

            ASIGNACION.Rule = id + igual + OP + puntoComa
                | id + igual + OP;

            MOSTRAR.Rule = mostrar + parentesisA + DatosMOSTRAR + parentesisC + puntoComa;

            DatosMOSTRAR.Rule = DatosMOSTRAR + coma + OP
                | OP;

            DibujarAST.Rule = dibujarAST + parentesisA + id + parentesisC + puntoComa;

            DibujarEXP.Rule = dibujarEXP + parentesisA + OP + parentesisC + puntoComa;

            INSTRUCCIONES.Rule = INSTRUCCIONES + INSTRUCCION
                | INSTRUCCION;

            INSTRUCCION.Rule = FUNCION
                | MAIN
                | METODO
                | DECLARACION
                | MOSTRAR
                | RETORNAR
                | CICLO
                | SI
                | ASIGNACION
                | SWITCH
                | LLAMADA
                | FOR
                | DibujarAST
                | DibujarEXP
                | detener + puntoComa
                | continuar + puntoComa;

            INSTRUCCION.ErrorRule = SyntaxError + ";"
                |SyntaxError + "}";

            METODO.Rule = tipoVoid + id + parentesisA + varPARAMETRO + parentesisC + llaveA + INSTRUCCIONES + llaveC
                | tipoVoid + id + parentesisA + parentesisC + llaveA + INSTRUCCIONES + llaveC
                | tipoVoid + id + parentesisA + parentesisC + llaveA + llaveC;

            FUNCION.Rule = TIPO + id + parentesisA + varPARAMETRO + parentesisC + llaveA + INSTRUCCIONES + llaveC
                  | TIPO + id + parentesisA + parentesisC + llaveA + INSTRUCCIONES + llaveC
                  | TIPO + id + parentesisA + varPARAMETRO + parentesisC + llaveA + llaveC
                  | TIPO + id + parentesisA + parentesisC + llaveA + llaveC;

            varPARAMETRO.Rule = varPARAMETRO + coma + TIPO + id
                  | TIPO + id;

            RETORNAR.Rule = retornar + OP + puntoComa
                | retornar + OP;
                //| retornar + puntoComa;

            MAIN.Rule = principal + parentesisA + parentesisC + llaveA + INSTRUCCIONES + llaveC;

            LLAMADA.Rule = id + parentesisA + TipoPARAMETRO + parentesisC + puntoComa
                     | id + parentesisA + TipoPARAMETRO + parentesisC
                     | id + parentesisA + parentesisC + puntoComa
                     | id + parentesisA + parentesisC;

            TipoPARAMETRO.Rule = TipoPARAMETRO + coma + OP
                        | OP;

            SI.Rule = si + parentesisA + OP + parentesisC + llaveA + INSTRUCCIONES + llaveC
                  | si + parentesisA + OP + parentesisC + llaveA + INSTRUCCIONES + llaveC + ELSE;

            ELSE.Rule = sino + INSTRUCCIONES
                | sino + llaveA + INSTRUCCIONES + llaveC;

            CICLO.Rule = hasta + parentesisA + OP + parentesisC + llaveA + INSTRUCCIONES + llaveC
                |mientras + parentesisA + OP + parentesisC + llaveA + INSTRUCCIONES + llaveC;

            SWITCH.Rule = selecciona + parentesisA + OP + parentesisC + CASE
                       | selecciona + parentesisA + OP + parentesisC + CASE + DEFAULT
                       | selecciona + parentesisA + OP + parentesisC + DEFAULT;

            CASE.Rule = CASE + OP + dosPuntos + llaveA + INSTRUCCIONES + llaveC
                | OP + dosPuntos + llaveA + INSTRUCCIONES + llaveC;

            DEFAULT.Rule = defecto + dosPuntos + llaveA + INSTRUCCIONES + llaveC;

            FOR.Rule = para + parentesisA + tipoNumber + id + igual + OP + puntoComa + OP + puntoComa + aumentar + parentesisC + llaveA + INSTRUCCIONES + llaveC
                | para + parentesisA + tipoNumber + id + igual + OP + puntoComa + OP + puntoComa + disminuir + parentesisC + llaveA + INSTRUCCIONES + llaveC;

            OP.Rule = E;

            L.Rule = parentesisA + L + parentesisC
                    | L + or + L
                    | L + and + L
                    | not + L
                    | OpRELACIONAL;

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
                | E +disminuir
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


            #region Preferencias
            this.Root = ENTRADA;
            //MarkPunctuation(si,num);
            // MarkPunctuation(INSTRUCCIONES);

            //Para esto utilizaremos la funcion RegisterOperators
            //El primer parametro indica el nivel de precedencia del 
            //operador(tiene mas precedencia  "||" que "^") 
            //tambien se indica el tipo de asociatividad que es izquierda para este caso
            // y se agrega el operador entre "".

            this.RegisterOperators(1, Associativity.Left, "==");
             this.RegisterOperators(2, Associativity.Left, "+", "-");
            this.RegisterOperators(3, Associativity.Left, "*", "/","%");
            this.RegisterOperators(4, Associativity.Right, "^");
            //this.RegisterOperators(4, Associativity.Neutral, "- ");
            this.RegisterOperators(5, Associativity.Left, "!=","<",">","<=",">=","~");
            this.RegisterOperators(6, Associativity.Left, "||");
            this.RegisterOperators(7, Associativity.Left, "!&");
            this.RegisterOperators(8, Associativity.Left, "&&");
            this.RegisterOperators(9, Associativity.Left, "!");
            // this.RegisterOperators(6, Associativity.Left, "==", "!=");
            #endregion
        }
    }
}
