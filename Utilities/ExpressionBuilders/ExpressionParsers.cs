namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um leitor de expressões que definem inteiros.
    /// </summary>
    /// <remarks>
    /// Por exemplo: "1+2*(3-4)+(-5)".
    /// </remarks>
    public class IntegerExpressionParser : IParse<int, string, string>
    {
        /// <summary>
        /// O leitor de expressões.
        /// </summary>
        private ExpressionReader<int, string, string> expressionReader;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="IntegerExpressionParser"/>.
        /// </summary>
        public IntegerExpressionParser()
        {
            this.expressionReader = new ExpressionReader<int, string, string>(
                new IntegerParser<string>());
            this.expressionReader.RegisterBinaryOperator("plus", Add, 0);
            this.expressionReader.RegisterBinaryOperator("times", Multiply, 1);
            this.expressionReader.RegisterBinaryOperator("minus", Subtract, 0);
            this.expressionReader.RegisterUnaryOperator("minus", Symmetric, 0);
            this.expressionReader.RegisterBinaryOperator("over", Divide, 1);
            this.expressionReader.RegisterBinaryOperator("mod", Remainder, 1);
            this.expressionReader.RegisterBinaryOperator("hat", Power, 2);
            this.expressionReader.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            this.expressionReader.RegisterSequenceDelimiterTypes("left_parenthesis", "right_parenthesis");
            this.expressionReader.AddVoid("blancks");
            this.expressionReader.AddVoid("space");
            this.expressionReader.AddVoid("carriage_return");
            this.expressionReader.AddVoid("new_line");
        }

        /// <summary>
        /// Tenta efectuar a leitura.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos a ler.</param>
        /// <param name="value">O valor que contém a leitura.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out int value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.expressionReader.TryParse(arrayReader, out value);
        }

        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public int Parse(
            ISymbol<string, string>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var arrayReader = new ArraySymbolReader<string, string>(
                symbolListToParse,
                Utils.GetStringSymbolType(EStringSymbolReaderType.EOF));
            var value = this.expressionReader.Parse(arrayReader, errorLogs);
            return value;
        }

        /// <summary>
        /// Efectua a operação de soma.
        /// </summary>
        /// <param name="i">O primeiro argumento.</param>
        /// <param name="j">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private int Add(int i, int j)
        {
            return i + j;
        }

        /// <summary>
        /// Efectua a operação de diferença.
        /// </summary>
        /// <param name="i">O primeiro argumento.</param>
        /// <param name="j">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private int Subtract(int i, int j)
        {
            return i - j;
        }

        /// <summary>
        /// Efectua a operação de produto.
        /// </summary>
        /// <param name="i">O primeiro argumento.</param>
        /// <param name="j">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private int Multiply(int i, int j)
        {
            return i * j;
        }

        /// <summary>
        /// Efectua a operação de soma.
        /// </summary>
        /// <param name="i">O primeiro argumento.</param>
        /// <param name="j">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private int Divide(int i, int j)
        {
            return i / j;
        }

        /// <summary>
        /// Efectua a operação de simétrico.
        /// </summary>
        /// <param name="i">O argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private int Symmetric(int i)
        {
            return -i;
        }

        /// <summary>
        /// Efectua a operação de resto da divisão.
        /// </summary>
        /// <param name="dividend">O primeiro argumento.</param>
        /// <param name="divisor">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private int Remainder(int dividend, int divisor)
        {
            return dividend % divisor;
        }

        /// <summary>
        /// Efectua a operação de potência.
        /// </summary>
        /// <param name="value">O primeiro argumento.</param>
        /// <param name="exponent">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        /// <exception cref="UtilitiesException">Se o expoente for negativo.</exception>
        private int Power(int value, int exponent)
        {
            if (exponent < 0)
            {
                throw new UtilitiesException("Negative exponents aren't allowed.");
            }
            else
            {
                return (int)Math.Pow(value, exponent);
            }
        }
    }

    /// <summary>
    /// Implementa um leitor para expressões com números de precisão dupla.
    /// </summary>
    /// <remarks>
    /// Por exemplo: "1.2-3.4*(0.1-1)".
    /// </remarks>
    public class DoubleExpressionParser : IParse<double, string, string>
    {
        /// <summary>
        /// O leitor de expressões.
        /// </summary>
        private ExpressionReader<double, string, string> expressionReader;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="DoubleExpressionParser"/>.
        /// </summary>
        public DoubleExpressionParser()
        {
            this.expressionReader = new ExpressionReader<double, string, string>(
                new DoubleParser<string>());
            this.expressionReader.RegisterBinaryOperator("plus", Add, 0);
            this.expressionReader.RegisterBinaryOperator("times", Multiply, 1);
            this.expressionReader.RegisterBinaryOperator("minus", Subtract, 0);
            this.expressionReader.RegisterUnaryOperator("minus", Symmetric, 0);
            this.expressionReader.RegisterBinaryOperator("over", Divide, 1);
            this.expressionReader.RegisterBinaryOperator("hat", Power, 2);
            this.expressionReader.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            this.expressionReader.RegisterSequenceDelimiterTypes("left_parenthesis", "right_parenthesis");
            this.expressionReader.AddVoid("blancks");
            this.expressionReader.AddVoid("space");
            this.expressionReader.AddVoid("carriage_return");
            this.expressionReader.AddVoid("new_line");
        }

        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public double Parse(
            ISymbol<string, string>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var arrayReader = new ArraySymbolReader<string, string>(
                symbolListToParse,
                Utils.GetStringSymbolType(EStringSymbolReaderType.EOF));
            var value = this.expressionReader.Parse(arrayReader, errorLogs);
            return value;
        }

        /// <summary>
        /// A função responsável pela operação soma.
        /// </summary>
        /// <param name="i">O primeiro argumento.</param>
        /// <param name="j">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private double Add(double i, double j)
        {
            return i + j;
        }

        /// <summary>
        /// A função responsável pela operação diferença.
        /// </summary>
        /// <param name="i">O primeiro argumento.</param>
        /// <param name="j">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private double Subtract(double i, double j)
        {
            return i - j;
        }

        /// <summary>
        /// A função responsável pela operação produto.
        /// </summary>
        /// <param name="i">O primeiro argumento.</param>
        /// <param name="j">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private double Multiply(double i, double j)
        {
            return i * j;
        }

        /// <summary>
        /// A função responsável pela operação divisão.
        /// </summary>
        /// <param name="i">O primeiro argumento.</param>
        /// <param name="j">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        /// <exception cref="DivideByZeroException">Em caso de divisão por zero.</exception>
        private double Divide(double i, double j)
        {
            if (j == 0)
            {
                throw new DivideByZeroException();
            }

            return i / j;
        }

        /// <summary>
        /// A função responsável pela operação simétrico.
        /// </summary>
        /// <param name="i">O argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private double Symmetric(double i)
        {
            return -i;
        }

        /// <summary>
        /// A função responsável pela operação potência.
        /// </summary>
        /// <param name="value">O primeiro argumento.</param>
        /// <param name="exponent">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private double Power(double value, double exponent)
        {
            return Math.Pow(value, exponent);
        }
    }

    /// <summary>
    /// Implementa um leitor de valores verdadeiro ou falso com base numa expressão lógica.
    /// </summary>
    /// <remarks>Um exemplo de expressão válida: "true || ~ false &amp;&amp; true".</remarks>
    public class BoolExpressionParser : IParse<bool, string, string>
    {
        /// <summary>
        /// O leitor de expressões.
        /// </summary>
        private ExpressionReader<bool, string, string> expressionReader;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BoolExpressionParser"/>.
        /// </summary>
        public BoolExpressionParser()
        {
            this.expressionReader = new ExpressionReader<bool, string, string>(
                new BoolParser<string>());
            this.expressionReader.RegisterBinaryOperator("double_or", Or, 0);
            this.expressionReader.RegisterBinaryOperator("double_and", And, 1);
            this.expressionReader.RegisterUnaryOperator("tild", Not, 0);
            this.expressionReader.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            this.expressionReader.AddVoid("blancks");
            this.expressionReader.AddVoid("space");
            this.expressionReader.AddVoid("carriage_return");
            this.expressionReader.AddVoid("new_line");
        }

        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public bool Parse(
            ISymbol<string, string>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var arrayReader = new ArraySymbolReader<string, string>(
                symbolListToParse,
                Utils.GetStringSymbolType(EStringSymbolReaderType.EOF));
            var value = this.expressionReader.Parse(arrayReader, errorLogs);
            return value;
        }

        /// <summary>
        /// A operaçõa responsável pela operação lógica "ou".
        /// </summary>
        /// <param name="i">O primeiro argumento.</param>
        /// <param name="j">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private bool Or(bool i, bool j)
        {
            return i || j;
        }

        /// <summary>
        /// A operaçõa responsável pela operação lógica "e".
        /// </summary>
        /// <param name="i">O primeiro argumento.</param>
        /// <param name="j">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private bool And(bool i, bool j)
        {
            return i && j;
        }

        /// <summary>
        /// A operaçõa responsável pela operação lógica "negação".
        /// </summary>
        /// <param name="i">O argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private bool Not(bool i)
        {
            return !i;
        }
    }
}
