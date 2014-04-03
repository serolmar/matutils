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
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <param name="coeffsParser">O leitor de representações textuais para os coeficientes.</param>
        /// <param name="conversion">A conversão do tipo do coeficientes para inteiro.</param>
        /// <param name="variableName">O nome da variável.</param>
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
                "x",
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
        /// Permite realizar a leitura de um polinómio a partir de uma representação textual.
        /// </summary>
        /// <typeparam name="T">O tipo dos coeficientes do polinómio.</typeparam>
        /// <param name="polynomialRepresentation">A representação textual do polinómio.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <param name="coeffsParser">O leitor de representações textuais para os coeficientes.</param>
        /// <param name="conversion">A conversão do tipo do coeficientes para inteiro.</param>
        /// <param name="variableName">O nome da variável.</param>
        /// <param name="externalDelimitersTypes">Os delimitadores externos.</param>
        /// <param name="readNegativeNumbers">Indica se o leitor identifica números negativos.</param>
        /// <returns>O polinómio lido a partir da representação textual.</returns>
        public static UnivariatePolynomialNormalForm<T> ReadUnivarPolynomial<T>(
            string polynomialRepresentation,
            IRing<T> ring,
            IParse<T, string, string> coeffsParser,
            IConversion<int, T> conversion,
            string variableName,
            Dictionary<string,string> externalDelimitersTypes,
            bool readNegativeNumbers = false)
        {
            var polInputReader = new StringReader(polynomialRepresentation);
            var polSymbolReader = new StringSymbolReader(polInputReader, readNegativeNumbers);
            var polParser = new UnivariatePolynomialReader<T, CharSymbolReader<string>>(
                "x",
                coeffsParser,
                ring);

            foreach (var kvp in externalDelimitersTypes)
            {
                polParser.RegisterExternalDelimiterTypes(kvp.Key, kvp.Value);
            }

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
        /// Permit realizar a leitura de um polinómio com coeficientes fraccionários.
        /// </summary>
        /// <typeparam name="T">O tipo de dados dos componentes das fracções.</typeparam>
        /// <param name="polynomialRepresentation">A representação polinomial.</param>
        /// <param name="domain">O domínio responsável pelas operações sobre os elementos das fracções.</param>
        /// <param name="itemsParser">O leitor de elementos da fracção.</param>
        /// <param name="conversion">A conversão entre cada fracção e o valor inteiro.</param>
        /// <param name="variableName">O nome da variável.</param>
        /// <param name="readNegativeNumbers">Indica se são lidos os números negativos.</param>
        /// <returns>O polinómio lido.</returns>
        public static UnivariatePolynomialNormalForm<Fraction<T>> ReadFractionalCoeffsUnivarPol<T, D>(
            string polynomialRepresentation,
            D domain,
            IParse<T,string,string> itemsParser,
            IConversion<int, Fraction<T>> conversion,
            string variableName,
            bool readNegativeNumbers = false) where D : IEuclidenDomain<T>
        {
            var fractionField = new FractionField<T>(domain);
            var fractionParser = new FieldDrivenExpressionParser<Fraction<T>>(
                new SimpleElementFractionParser<T>(itemsParser, domain), 
                fractionField);
            var polInputReader = new StringReader(polynomialRepresentation);
            var polSymbolReader = new StringSymbolReader(polInputReader, readNegativeNumbers);
            var polParser = new UnivariatePolynomialReader<Fraction<T>, CharSymbolReader<string>>(
                "x",
                fractionParser,
                fractionField);

            var result = default(UnivariatePolynomialNormalForm<Fraction<T>>);
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
        /// Permite fazer a leitura de uma matriz.
        /// </summary>
        /// <typeparam name="T">O tipo de elementos na matriz.</typeparam>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="matrixText">A representação textual da matriz.</param>
        /// <param name="matrixFactory">A fábrica responsável pela criação de matrizes.</param>
        /// <param name="elementParser">O leitor de elementos.</param>
        /// <param name="readNegativeNumbers">Indica se são lidos os números negativos.</param>
        /// <returns>A matriz.</returns>
        public static IMatrix<T> ReadMatrix<T>(
            int lines, 
            int columns, 
            string matrixText, 
            IMatrixFactory<T> matrixFactory, 
            IParse<T, string, string> elementParser,
            bool readNegativeNumbers = false)
        {
            var reader = new StringReader(matrixText);
            var stringSymbolReader = new StringSymbolReader(reader, readNegativeNumbers);
            var arrayMatrixReader = new ConfigMatrixReader<T, string, string, CharSymbolReader<string>>(
                lines,
                columns,
                matrixFactory);
            arrayMatrixReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayMatrixReader.AddBlanckSymbolType("blancks");
            arrayMatrixReader.SeparatorSymbType = "comma";

            var matrix = default(IMatrix<T>);
            if (arrayMatrixReader.TryParseMatrix(stringSymbolReader, elementParser, out matrix))
            {
                return matrix;
            }
            else
            {
                throw new Exception("Can't read matrix.");
            }
        }

        /// <summary>
        /// Permite fazer a leitura de um vector.
        /// </summary>
        /// <typeparam name="T">O tipo de elementos do vector.</typeparam>
        /// <param name="dimension">A dimensão do vector a ser lido.</param>
        /// <param name="vectorText">O texto que representa o vector.</param>
        /// <param name="vectorFactory">A fábrica responsável pela criação de vectores.</param>
        /// <param name="elementParser">O leitor de elementos.</param>
        /// <param name="readNegativeNumbers">Indica se são lidos os números negativos.</param>
        /// <returns></returns>
        public static IVector<T> ReadVector<T>(
            int dimension,
            string vectorText,
            IVectorFactory<T> vectorFactory,
            IParse<T, string, string> elementParser,
            bool readNegativeNumbers = false)
        {
            var reader = new StringReader(vectorText);
            var stringSymbolReader = new StringSymbolReader(reader, readNegativeNumbers);
            var arrayVectorReader = new ConfigVectorReader<T, string, string, CharSymbolReader<string>>(
                dimension,
                vectorFactory);
            arrayVectorReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayVectorReader.AddBlanckSymbolType("blancks");
            arrayVectorReader.SeparatorSymbType = "comma";

            var vector = default(IVector<T>);
            if (arrayVectorReader.TryParseVector(stringSymbolReader, elementParser, out vector))
            {
                return vector;
            }
            else
            {
                throw new Exception("Can't read vector.");
            }
        }
    }
}
