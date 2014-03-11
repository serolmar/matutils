namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities;

    public class IntegerFractionPolReader
    {
        /// <summary>
        /// Permite efectuar a leitura de um polinómio com coeficientes fraccionários a partir de texto.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem sucedida, é lançada uma excep~ção.
        /// </remarks>
        /// <param name="polynomial">O texto.</param>
        /// <returns>O polinómio.</returns>
        public UnivariatePolynomialNormalForm<Fraction<int, IntegerDomain>> Read(string polynomial)
        {
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int, IntegerDomain>(integerDomain);
            var integerParser = new IntegerParser<string>();
            var fractionParser = new FractionExpressionParser<int, IntegerDomain>(integerParser, fractionField);
            var conversion = new ElementFractionConversion<int, IntegerDomain>(integerDomain);
            var polInputReader = new StringReader(polynomial);
            var polSymbolReader = new StringSymbolReader(polInputReader, false);
            var polParser = new UnivariatePolynomialReader<Fraction<int, IntegerDomain>, CharSymbolReader<string>>(
                "x",
                fractionParser,
                fractionField);

            var result = default(UnivariatePolynomialNormalForm<Fraction<int, IntegerDomain>>);
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
