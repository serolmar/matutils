namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

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
        /// Tenta fazer a leitura da expressão.
        /// </summary>
        /// <param name="symbolListToParse">A lista da símbolos a ler.</param>
        /// <param name="value">Valor que recebe a leitura.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out bool value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.expressionReader.TryParse(arrayReader, out value);
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
