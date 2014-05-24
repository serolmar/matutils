using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Mathematics
{
    /// <summary>
    /// Implementa um leitor de polinómios univariáveis.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os coeficientes dos polinómios univariáveis.</typeparam>
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
        /// O nome da variável.
        /// </summary>
        private string variableName;

        /// <summary>
        /// O anel responsável pela aplicação das operações sobre os coeficientes.
        /// </summary>
        private IRing<CoeffType> coeffRing;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimpleUnivarPolynomNormalFormReader{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffParser">O leitor de coeficientes.</param>
        /// <param name="coeffRing">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <param name="variableName">O nome da variável.</param>
        /// <exception cref="MathematicsException">Se um leitor de coeficientes não for providenciado.</exception>
        public SimpleUnivarPolynomNormalFormReader(
            IParse<CoeffType, string, string> coeffParser, 
            IRing<CoeffType> coeffRing,
            string variableName)
        {
            if (coeffParser == null)
            {
                throw new MathematicsException("A coefficient parser must be provided.");
            }
            else if (coeffRing == null)
            {
                throw new MathematicsException("A coefficient ring must be provided.");
            }
            else
            {
                this.coeffParser = coeffParser;
                this.coeffRing = coeffRing;
                this.variableName = variableName;
            }
        }

        /// <summary>
        /// Efectua a leitura de um termo polinomial.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos para leitura.</param>
        /// <param name="pol">A variável que recebe o polinómio lido.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(
            ISymbol<string, string>[] symbolListToParse, 
            out ParseUnivarPolynomNormalFormItem<CoeffType> pol)
        {
            pol = null;
            var parsedCoeff = default(CoeffType);
            
            if (symbolListToParse.Length == 1)
            {
                var stringValue = symbolListToParse[0].SymbolValue;
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return false;
                }
                else if (char.IsLetter(stringValue[0]))
                {
                    if (stringValue == this.variableName)
                    {
                        pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                        pol.Polynomial = new UnivariatePolynomialNormalForm<CoeffType>(
                            this.coeffRing.MultiplicativeUnity,
                            1,
                            stringValue,
                            this.coeffRing);
                        return true;
                    }
                    else if (this.coeffParser.TryParse(symbolListToParse, out parsedCoeff))
                    {
                        pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                        pol.Coeff = parsedCoeff;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (this.coeffParser.TryParse(symbolListToParse, out parsedCoeff))
                {
                    pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                    pol.Coeff = parsedCoeff;
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
            else if (this.coeffParser.TryParse(symbolListToParse, out parsedCoeff))
            {
                pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                pol.Coeff = parsedCoeff;
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
    }
}
