namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using Mathematics;
    using Utilities;

    public class BigIntegerPolynomialReader
    {
        /// <summary>
        /// Permite efectuar a leitura de um polinómio a partir de texto.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem sucedida, é lançada uma excep~ção.
        /// </remarks>
        /// <param name="polynomial">O texto.</param>
        /// <returns>O polinómio.</returns>
        public UnivariatePolynomialNormalForm<BigInteger> Read(string polynomial)
        {
            var integerDomain = new BigIntegerDomain();
            var integerParser = new BigIntegerParser<string>();
            var conversion = new BigIntegerToIntegerConversion();
            var polInputReader = new StringReader(polynomial);
            var polSymbolReader = new StringSymbolReader(polInputReader, false);
            var polParser = new UnivariatePolynomialReader<BigInteger, CharSymbolReader<string>>(
                "x",
                integerParser,
                integerDomain);

            var result = default(UnivariatePolynomialNormalForm<BigInteger>);
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
