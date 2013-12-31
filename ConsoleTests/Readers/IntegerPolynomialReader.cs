namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities.Parsers;
    using System.IO;

    public class IntegerPolynomialReader
    {
        /// <summary>
        /// Permite efectuar a leitura de um polinómio a partir de texto.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem sucedida, é lançada uma excep~ção.
        /// </remarks>
        /// <param name="polynomial">O texto.</param>
        /// <returns>O polinómio.</returns>
        public UnivariatePolynomialNormalForm<int> Read(string polynomial)
        {
            var integerDomain = new IntegerDomain();
            var integerParser = new IntegerParser<string>();
            var conversion = new ElementToElementConversion<int>();
            var polInputReader = new StringReader(polynomial);
            var polSymbolReader = new StringSymbolReader(polInputReader, false);
            var polParser = new UnivariatePolynomialReader<int, CharSymbolReader<string>>(
                "x",
                integerParser,
                integerDomain);

            var result = default(UnivariatePolynomialNormalForm<int>);
            if (polParser.TryParsePolynomial(polSymbolReader, conversion, out result))
            {
                // O polinómio foi lido com sucesso.
                return result;
            }
            else
            {
                // Não é possível ler o polinómio.
                throw new Exception("Can't read integer polynomial.");
            }
        }
    }
}
