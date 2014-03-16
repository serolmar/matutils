namespace Mathematics.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa diversas funções que poderão auxiliar os testes.
    /// </summary>
    public static class TestsHelper
    {
        /// <summary>
        /// Permite realizar a leitura de um polinómio a partir de uma representação textual.
        /// </summary>
        /// <typeparam name="T">O tipo dos coeficientes do polinómio.</typeparam>
        /// <param name="polynomialRepresentation">A representação textual do polinómio.</param>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <param name="coeffsParser">O leitor de representações textuais para os coeficientes.</param>
        /// <param name="conversion">A conversão do tipo do coeficientes para inteiro.</param>
        /// <param name="variableName">O nome da variável.</param>
        /// <returns>O polinómio lido a partir da representação textual.</returns>
        public static UnivariatePolynomialNormalForm<T> ReadUnivarPolynomial<T>(
            string polynomialRepresentation,
            IEuclidenDomain<T> domain,
            IParse<T, string, string> coeffsParser,
            IConversion<int, T> conversion,
            string variableName)
        {
            var polInputReader = new StringReader(polynomialRepresentation);
            var polSymbolReader = new StringSymbolReader(polInputReader, false);
            var polParser = new UnivariatePolynomialReader<T, CharSymbolReader<string>>(
                "x",
                coeffsParser,
                domain);

            var result = default(UnivariatePolynomialNormalForm<T>);
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
