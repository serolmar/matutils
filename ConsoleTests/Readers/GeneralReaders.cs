using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mathematics;
using Utilities;

namespace ConsoleTests
{
    class GeneralReaders
    {
        /// <summary>
        /// Permite realizar a leitura de um polinómio a partir de uma representação textual.
        /// </summary>
        /// <typeparam name="T">O tipo dos coeficientes do polinómio.</typeparam>
        /// <param name="polynomialRepresentation">A representação textual do polinómio.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <param name="coeffsParser">O leitor de representações textuais para os coeficientes.</param>
        /// <param name="conversion">A conversão do tipo do coeficientes para inteiro.</param>
        /// <param name="variableName">O nome da variável.</param>
        /// <param name="readNegativeNumbers">
        /// Indica se os números negativos são para ser lidos ou se é lido
        /// o sinal e depois o respectivo valor como símbolos independentes.</param>
        /// <returns>O polinómio lido a partir da representação textual.</returns>
        public static UnivariatePolynomialNormalForm<T> ReadUnivarPolynomial<T>(
            string polynomialRepresentation,
            IRing<T> ring,
            IParse<T, string, string> coeffsParser,
            IConversion<int, T> conversion,
            string variableName,
            bool readNegativeNumbers = false)
        {
            var polInputReader = new StringReader(polynomialRepresentation);
            var polSymbolReader = new StringSymbolReader(polInputReader, readNegativeNumbers);
            var polParser = new UnivariatePolynomialReader<T, CharSymbolReader<string>>(
                variableName,
                coeffsParser,
                ring);

            var result = default(UnivariatePolynomialNormalForm<T>);
            if (polParser.TryParsePolynomial(polSymbolReader, conversion, out result))
            {
                // O polinómio foi lido com sucesso.
                return result;
            }
            else
            {
                // Não é possível ler o polinómio.
                throw new Exception("Can't read polynomial.");
            }
        }

        /// <summary>
        /// Permite realizar a leitura de um polinómio bivariável a partir de uma representação textual.
        /// </summary>
        /// <remarks>
        /// A leitura permite obter um polinómio cujos coeficientes são outros polinómios.
        /// </remarks>
        /// <typeparam name="T">O tipo dos objectos que constituem os coeficientes.</typeparam>
        /// <param name="polynomialRepresentation">A representação polinomial.</param>
        /// <param name="polVariable">A variável do polinómio.</param>
        /// <param name="coeffsVariable">A variável dos polinómios que servem de coeficientes.</param>
        /// <param name="coeffsDegConversion">O conversor dos coeficientes para inteiros.</param>
        /// <param name="innerCoeffsParser">O leitor dos coeficientes.</param>
        /// <param name="innerCoeffsRing">O anel resonsável pelas operaçoes sobre os coeficientes.</param>
        /// <param name="readNegativeNumbers">
        /// Verdadeiro caso seja para ler números negativos e falso caso contrário.
        /// </param>
        /// <returns></returns>
        public static UnivariatePolynomialNormalForm<UnivariatePolynomialNormalForm<T>> ReadUnivarPolWithPolCoeffs<T>(
            string polynomialRepresentation,
            string polVariable,
            string coeffsVariable,
            IConversion<int,T> coeffsDegConversion,
            IParse<T, string, string> innerCoeffsParser,
            IRing<T> innerCoeffsRing,
            bool readNegativeNumbers = false)
        {
            var polInputReader = new StringReader(polynomialRepresentation);
            var polSymbolReader = new StringSymbolReader(polInputReader, readNegativeNumbers);

            var coeffsPolParser = new UnivarPolNormalFormParser<T>(
                coeffsVariable,
                coeffsDegConversion,
                innerCoeffsParser,
                innerCoeffsRing
                );

            var polRing = new UnivarPolynomRing<T>(
                coeffsVariable,
                innerCoeffsRing);

            var polParser = new UnivariatePolynomialReader<UnivariatePolynomialNormalForm<T>, CharSymbolReader<string>>(
                polVariable,
                coeffsPolParser,
                polRing);

            var polConversion = new UnivarPolynomNormalFormToIntegerConversion<T>(
                coeffsVariable,
                coeffsDegConversion,
                innerCoeffsRing);
            var result = default(UnivariatePolynomialNormalForm<UnivariatePolynomialNormalForm<T>>);
            if (polParser.TryParsePolynomial(polSymbolReader, polConversion, out result))
            {
                // O polinómio foi lido com sucesso.
                return result;
            }
            else
            {
                // Não é possível ler o polinómio.
                throw new Exception("Can't read polynomial.");
            }
        }
    }
}
