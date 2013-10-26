using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Mathematics
{
    class SimpleUnivarPolynomNormalFormReader<CoeffType> :
        IParse<ParseUnivarPolynomNormalFormItem<CoeffType>, string, string>
    {
        /// <summary>
        /// O leitor de coeficientes.
        /// </summary>
        private IParse<CoeffType, string, string> coeffParser;

        /// <summary>
        /// O leitor de inteiros.
        /// </summary>
        private IParse<int, string, string> integerParser = new IntegerParser<string>();

        /// <summary>
        /// O anel responsável pela aplicação das operações sobre os coeficientes.
        /// </summary>
        private IRing<CoeffType> coeffRing;

        public SimpleUnivarPolynomNormalFormReader(
            IParse<CoeffType, string, string> coeffParser, 
            IRing<CoeffType> coeffRing)
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
        public bool TryParse(
            ISymbol<string, string>[] symbolListToParse, 
            out ParseUnivarPolynomNormalFormItem<CoeffType> pol)
        {
            pol = null;
            var parsedCoeff = default(CoeffType);
            if (this.coeffParser.TryParse(symbolListToParse, out parsedCoeff))
            {
                pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
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
                    pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                    pol.Polynomial = new UnivariatePolynomialNormalForm<CoeffType>(
                        this.coeffRing.MultiplicativeUnity,
                        1,
                        stringValue,
                        this.coeffRing);
                    return true;
                }
                else
                {
                    var integerValue = 0;
                    if (this.integerParser.TryParse(symbolListToParse, out integerValue))
                    {
                        pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                        pol.Degree = integerValue;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                var integerValue = 0;
                if (this.integerParser.TryParse(symbolListToParse, out integerValue))
                {
                    pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                    pol.Degree = integerValue;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
