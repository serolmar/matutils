namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class UnivarPolNormalFormParser<CoeffType> 
        : IParse<UnivariatePolynomialNormalForm<CoeffType>, string, string>
    {
        /// <summary>
        /// O anel que contém as operações a serem efectuadas sobre o polinómio.
        /// </summary>
        private IRing<CoeffType> coefficientsRing;

        /// <summary>
        /// O leitor de matrizes multidimensionais.
        /// </summary>
        private UnivariatePolynomialReader<CoeffType, ISymbol<string, string>[]> polynomialReader;

        /// <summary>
        /// O leitor dos elmentos contidos na matriz multidimensional.
        /// </summary>
        private IParse<CoeffType, string, string> elementsParser;

        /// <summary>
        /// O conversor entre os coeficientes e os inteiros.
        /// </summary>
        private IConversion<int, CoeffType> conversion;

        /// <summary>
        /// O nome da variável.
        /// </summary>
        private string variable;

        public UnivarPolNormalFormParser(
            string variable, 
            IConversion<int, CoeffType> conversion,
            IParse<CoeffType, string, string> elementsParser, IRing<CoeffType> ring)
        {
            if (string.IsNullOrWhiteSpace(variable))
            {
                throw new ArgumentException("Variable must hava a non empty value.");
            }
            else if (conversion == null)
            {
                throw new ArgumentNullException("conversion");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (elementsParser == null)
            {
                throw new ArgumentNullException("elementsParser");
            }
            else
            {
                this.variable = variable;
                this.conversion = conversion;
                this.coefficientsRing = ring;
                this.elementsParser = elementsParser;
                this.polynomialReader = new UnivariatePolynomialReader<CoeffType, ISymbol<string, string>[]>(
                    variable,
                    this.elementsParser,
                    this.coefficientsRing);
            }
        }

        public bool TryParse(
            ISymbol<string, string>[] symbolListToParse, 
            out UnivariatePolynomialNormalForm<CoeffType> value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.polynomialReader.TryParsePolynomial(arrayReader, this.conversion, out value);
        }

        /// <summary>
        /// Permite registar delimitadores de precedência na expressão.
        /// </summary>
        /// <remarks>
        /// Caso nenhum delimitador de expressão se encontre registado, serão utilizados os parêntesis
        /// de abertura e fecho por defeito.
        /// </remarks>
        /// <param name="openDelimiterType">O tipo do delimitador de abertura.</param>
        /// <param name="closeDelimiterType">O tiop do delimitador de fecho.</param>
        public void RegisterExpressionDelimitersTyes(string openDelimiterType, string closeDelimiterType)
        {
            this.polynomialReader.RegisterInteralDelimiterTypes(openDelimiterType, closeDelimiterType);
        }

        /// <summary>
        /// Elimmina todos os mapeamentos de expressão previamente registados.
        /// </summary>
        public void ClearExpressionDelimitersTypes()
        {
            this.polynomialReader.ClearInternalDelimitersMappings();
        }

        /// <summary>
        /// Permite associar tipos de delimitadores externos.
        /// </summary>
        /// <param name="openDelimiterType">O tiop de delimitador externo de abertura.</param>
        /// <param name="closeDelimiterType">O tipo de delimitador externo de fecho.</param>
        public void RegisterExternalDelimitersTypes(string openDelimiterType, string closeDelimiterType)
        {
            this.polynomialReader.RegisterExternalDelimiterTypes(openDelimiterType, closeDelimiterType);
        }

        /// <summary>
        /// Elimina todos os mapeamentos externos previamente registados.
        /// </summary>
        public void ClearExternalDelmitersTypes()
        {
            this.polynomialReader.ClearExternalDelimitersMappings();
        }
    }
}
