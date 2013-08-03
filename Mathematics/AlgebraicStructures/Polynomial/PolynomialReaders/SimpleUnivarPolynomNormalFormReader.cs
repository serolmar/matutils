using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Mathematics
{
    class SimpleUnivarPolynomNormalFormReader<CoeffType, RingType> :
        IParse<ParseUnivarPolynomNormalFormItem<CoeffType, RingType>, string, string>
        where RingType : IRing<CoeffType>
    {
        /// <summary>
        /// O leitor de coeficientes.
        /// </summary>
        private IParse<CoeffType, string, string> coeffParser;

        /// <summary>
        /// O leitor de inteiros.
        /// </summary>
        private IParse<int, string, string> integerParser = new IntegerParser();

        /// <summary>
        /// O anel responsável pela aplicação das operações sobre os coeficientes.
        /// </summary>
        private RingType coeffRing;

        public SimpleUnivarPolynomNormalFormReader(IParse<CoeffType, string, string> coeffParser, RingType coeffRing)
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
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out ParseUnivarPolynomNormalFormItem<CoeffType, RingType> pol)
        {
            var integerValue = 0;
            pol = null;
            if (this.integerParser.TryParse(symbolListToParse, out integerValue))
            {
                pol = new ParseUnivarPolynomNormalFormItem<CoeffType, RingType>();
                pol.Degree = integerValue;
                return true;
            }
            else
            {
                var parsedCoeff = default(CoeffType);
                if (this.coeffParser.TryParse(symbolListToParse, out parsedCoeff))
                {
                    pol = new ParseUnivarPolynomNormalFormItem<CoeffType, RingType>();
                    pol.Coeff = parsedCoeff;
                    return true;
                }
                else if (symbolListToParse.Length == 1)
                {
                    var stringValue = symbolListToParse[0].SymbolValue;
                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        return false;
                    }
                    else if (char.IsLetter(stringValue[0]))
                    {
                        pol = new ParseUnivarPolynomNormalFormItem<CoeffType, RingType>();
                        pol.Polynomial = new UnivariatePolynomialNormalForm<CoeffType, RingType>(
                            this.coeffRing.MultiplicativeUnity, 
                            1, 
                            stringValue, 
                            this.coeffRing);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
