namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um leitor de expressões que definem inteiros.
    /// </summary>
    /// <remarks>
    /// Por exemplo: "1+2*(3-4)+(-5)".
    /// </remarks>
    public class IntegerExpressionParser : IParse<int, string, string>, IParse<object, string, string>
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
        /// Tenta efectuar a leitura.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos a ler.</param>
        /// <param name="value">O valor que contém a leitura.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out object value)
        {
            var temp = default(int);
            if (this.TryParse(symbolListToParse, out temp))
            {
                value = temp;
                return true;
            }
            else
            {
                value = default(object);
                return false;
            }
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
}
