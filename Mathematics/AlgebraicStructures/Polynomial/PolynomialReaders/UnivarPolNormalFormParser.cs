namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;
    using Utilities.Parsers;

    public class UnivarPolNormalFormParser<CoeffType, RingType> : IParse<UnivariatePolynomialNormalForm<CoeffType, RingType>, string, string>
        where RingType : IRing<CoeffType>
    {
        /// <summary>
        /// O anel que contém as operações a serem efectuadas sobre o polinómio.
        /// </summary>
        private RingType coefficientsRing;

        /// <summary>
        /// O leitor de matrizes multidimensionais.
        /// </summary>
        private UnivariatePolynomialReader<CoeffType, RingType, ISymbol<string, string>[]> polynomialReader;

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
            IParse<CoeffType, string, string> elementsParser, RingType ring)
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
                this.polynomialReader = new UnivariatePolynomialReader<CoeffType, RingType, ISymbol<string, string>[]>(
                    variable,
                    this.elementsParser,
                    this.coefficientsRing);
            }
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out UnivariatePolynomialNormalForm<CoeffType, RingType> value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.polynomialReader.TryParsePolynomial(arrayReader, this.conversion, out value);
        }
    }
}
