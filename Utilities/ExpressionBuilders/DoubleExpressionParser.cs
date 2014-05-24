namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um leitor para expressões com números de precisão dupla.
    /// </summary>
    /// <remarks>
    /// Por exemplo: "1.2-3.4*(0.1-1)".
    /// </remarks>
    public class DoubleExpressionParser : IParse<double, string, string>, IParse<object, string, string>
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
        /// Tenta realizar a leitura.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos a ler.</param>
        /// <param name="value">O valor que contém a leitura.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out double value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.expressionReader.TryParse(arrayReader, out value);
        }

        /// <summary>
        /// Tenta realizar a leitura.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos a ler.</param>
        /// <param name="value">O valor que contém a leitura.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out object value)
        {
            var temp = default(double);
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
}
