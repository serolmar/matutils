﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    class PolynomialParser<CoeffType, RingType> : IParse<Polynomial<CoeffType, RingType>, string, string>
        where RingType : IRing<CoeffType>
    {
        /// <summary>
        /// O anel que contém as operações a serem efectuadas sobre o polinómio.
        /// </summary>
        private RingType coefficientsRing;

        /// <summary>
        /// O leitor de matrizes multidimensionais.
        /// </summary>
        private PolynomialReader<CoeffType, RingType, ISymbol<string, string>[]> polynomialReader;

        /// <summary>
        /// O leitor dos elmentos contidos na matriz multidimensional.
        /// </summary>
        private IParse<CoeffType, string, string> elementsParser;

        public PolynomialParser(IParse<CoeffType, string, string> elementsParser, RingType ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (elementsParser == null)
            {
                throw new ArgumentNullException("elementsParser");
            }
            else
            {
                this.coefficientsRing = ring;
                this.elementsParser = elementsParser;
                this.polynomialReader = new PolynomialReader<CoeffType, RingType, ISymbol<string, string>[]>(
                    this.elementsParser,
                    this.coefficientsRing);
            }
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out Polynomial<CoeffType, RingType> value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.polynomialReader.TryParsePolynomial(arrayReader, out value);
        }
    }
}
