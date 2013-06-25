namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    class SimplePolynomialParser<CoeffType, RingType> : IParse<Polynomial<CoeffType, RingType>, string, string>
        where RingType : IRing<CoeffType>
    {
        /// <summary>
        /// O parser de coeficientes.
        /// </summary>
        private IParse<CoeffType, string, string> coeffParser;

        /// <summary>
        /// O anel responsável pela aplicação das operações sobre os coeficientes.
        /// </summary>
        private RingType coeffRing;

        public SimplePolynomialParser(IParse<CoeffType, string, string> coeffParser, RingType coeffRing)
        {
            if (coeffParser == null)
            {
                throw new MathematicsException("A coefficient parser must be provided.");
            }

            if (coeffRing == null)
            {
                throw new MathematicsException("A coefficient ring must be provided.");
            }

            this.coeffParser = coeffParser;
            this.coeffRing = coeffRing;
        }

        /// <summary>
        /// Efectua a leitura de um termo polinomial.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos para leitura.</param>
        /// <returns>O polinómio requerido.</returns>
        public Polynomial<CoeffType, RingType> Parse(ISymbol<string, string>[] symbolListToParse)
        {
            if (symbolListToParse.Length > 1)
            {
                var errorBuilder = new StringBuilder();
                errorBuilder.Append("Invalid polynomial term:");
                foreach (var symbol in symbolListToParse)
                {
                    errorBuilder.AppendFormat(" {0}", symbol.SymbolValue);
                }

                throw new ExpressionReaderException(errorBuilder.ToString());
            }
            else
            {
                var firstSymol = symbolListToParse[0];
                if (string.IsNullOrWhiteSpace(firstSymol.SymbolValue))
                {
                    return new Polynomial<CoeffType, RingType>(this.coeffRing);
                }
                else
                {
                    if (firstSymol.SymbolValue.Any(s => !char.IsLetter(s)))
                    {
                        var parsedCoeff = this.coeffParser.Parse(new[] { firstSymol });
                        return new Polynomial<CoeffType, RingType>(parsedCoeff, this.coeffRing);
                    }
                    else
                    {
                        return new Polynomial<CoeffType, RingType>(this.coeffRing.MultiplicativeUnity, firstSymol.SymbolValue, this.coeffRing);
                    }
                }
            }
        }
    }
}
