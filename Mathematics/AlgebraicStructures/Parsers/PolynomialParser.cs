namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um leitor de polinómios.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constitem os coeficientes dos polinómios.</typeparam>
    class PolynomialParser<CoeffType> : IParse<Polynomial<CoeffType>, string, string>
    {
        /// <summary>
        /// O anel que contém as operações a serem efectuadas sobre o polinómio.
        /// </summary>
        private IRing<CoeffType> coefficientsRing;

        /// <summary>
        /// O leitor de matrizes multidimensionais.
        /// </summary>
        private PolynomialReader<CoeffType, ISymbol<string, string>[]> polynomialReader;

        /// <summary>
        /// O leitor dos elmentos contidos na matriz multidimensional.
        /// </summary>
        private IParse<CoeffType, string, string> elementsParser;

        /// <summary>
        /// O conversor entre os coeficientes e os inteiros.
        /// </summary>
        private IConversion<int, CoeffType> conversion;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="PolynomialParser{CoeffType}"/>.
        /// </summary>
        /// <param name="elementsParser">O leitor de coeficientes.</param>
        /// <param name="conversion">O objecto responsável pela conversão entre coeficientes e inteiros.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        public PolynomialParser(
            IParse<CoeffType, string, string> elementsParser, 
            IConversion<int, CoeffType> conversion,
            IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (conversion == null)
            {
                throw new ArgumentNullException("conversion");
            }
            else if (elementsParser == null)
            {
                throw new ArgumentNullException("elementsParser");
            }
            else
            {
                this.coefficientsRing = ring;
                this.conversion = conversion;
                this.elementsParser = elementsParser;
                this.polynomialReader = new PolynomialReader<CoeffType, ISymbol<string, string>[]>(
                    this.elementsParser,
                    this.coefficientsRing);
            }
        }

        /// <summary>
        /// Experimenta a leitura de um polinómio a partir de uma lista de símbolos.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos.</param>
        /// <param name="value">O valor que irá receber a leitura.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out Polynomial<CoeffType> value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.polynomialReader.TryParsePolynomial(arrayReader, this.conversion, out value);
        }
    }
}
