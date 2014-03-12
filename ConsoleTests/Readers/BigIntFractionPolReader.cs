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

    public class BigIntFractionPolReader
    {
        /// <summary>
        /// Permite efectuar a leitura de um polinómio com coeficientes fraccionários a partir de texto.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem sucedida, é lançada uma excep~ção.
        /// </remarks>
        /// <param name="polynomial">O texto.</param>
        /// <returns>O polinómio.</returns>
        public UnivariatePolynomialNormalForm<Fraction<BigInteger, IIntegerNumber<BigInteger>>> Read(string polynomial)
        {
            var integerDomain = new BigIntegerDomain();
            var fractionField = new FractionField<BigInteger, IIntegerNumber<BigInteger>>(integerDomain);
            var integerParser = new BigIntegerParser<string>();
            var fractionParser = new FractionExpressionParser<BigInteger, IIntegerNumber<BigInteger>>(integerParser, fractionField);
            var conversion = new IntegerBigIntFractionConversion(integerDomain, new BigIntegerToIntegerConversion());
            var polInputReader = new StringReader(polynomial);
            var polSymbolReader = new StringSymbolReader(polInputReader, false);
            var polParser = new UnivariatePolynomialReader<Fraction<BigInteger, IIntegerNumber<BigInteger>>, CharSymbolReader<string>>(
                "x",
                fractionParser,
                fractionField);

            var result = default(UnivariatePolynomialNormalForm<Fraction<BigInteger, IIntegerNumber<BigInteger>>>);
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
