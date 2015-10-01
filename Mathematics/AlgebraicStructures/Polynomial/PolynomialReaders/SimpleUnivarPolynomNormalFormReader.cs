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
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public ParseUnivarPolynomNormalFormItem<CoeffType> Parse(
            ISymbol<string, string>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var pol = default(ParseUnivarPolynomNormalFormItem<CoeffType>);
            if (symbolListToParse.Length == 1)
            {
                var stringValue = symbolListToParse[0].SymbolValue;
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    errorLogs.AddLog(
                        "The variable name can't be null nor empty.",
                        EParseErrorLevel.ERROR);
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
                    }
                    else
                    {
                        var coeffError = new LogStatus<string, EParseErrorLevel>();
                        var parsedCoeff = this.coeffParser.Parse(symbolListToParse, coeffError);
                        if (!coeffError.HasLogs(EParseErrorLevel.ERROR))
                        {
                            pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                            pol.Coeff = parsedCoeff;
                        }

                        // Poderão existir avisos.
                        this.CopyLogs(coeffError, errorLogs);
                    }
                }
                else
                {
                    var coeffError = new LogStatus<string, EParseErrorLevel>();
                    var parsedCoeff = this.coeffParser.Parse(symbolListToParse, coeffError);
                    if (coeffError.HasLogs(EParseErrorLevel.ERROR))
                    {
                        var degreeError = new LogStatus<string, EParseErrorLevel>();
                        var integerValue = this.integerParser.Parse(symbolListToParse, degreeError);
                        if (degreeError.HasLogs(EParseErrorLevel.ERROR))
                        {
                            this.CopyLogs(coeffError, errorLogs);
                        }
                        else
                        {
                            pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                            pol.Degree = integerValue;

                            // Poderão existir avisos
                            this.CopyLogs(degreeError, errorLogs);
                        }
                    }
                    else
                    {
                        pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                        pol.Coeff = parsedCoeff;

                        // Poderão existir avisos.
                        this.CopyLogs(coeffError, errorLogs);
                    }
                }
            }
            else
            {
                var coeffError = new LogStatus<string, EParseErrorLevel>();
                var parsedCoeff = this.coeffParser.Parse(symbolListToParse, coeffError);
                if (coeffError.HasLogs(EParseErrorLevel.ERROR))
                {
                    var degreeError = new LogStatus<string, EParseErrorLevel>();
                    var integerValue = this.integerParser.Parse(symbolListToParse, degreeError);
                    if (degreeError.HasLogs(EParseErrorLevel.ERROR))
                    {
                        this.CopyLogs(coeffError, errorLogs);
                    }
                    else
                    {
                        pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                        pol.Degree = integerValue;

                        // Poderão existir avisos
                        this.CopyLogs(degreeError, errorLogs);
                    }
                }
                else
                {
                    pol = new ParseUnivarPolynomNormalFormItem<CoeffType>();
                    pol.Coeff = parsedCoeff;

                    // Poderão existir avisos.
                    this.CopyLogs(coeffError, errorLogs);
                }
            }

            return pol;
        }

        /// <summary>
        /// Copia mensagens entre diários.
        /// </summary>
        /// <param name="source">O diário de origem.</param>
        /// <param name="destination">O diário de destino.</param>
        private void CopyLogs(
            ILogStatus<string, EParseErrorLevel> source,
            ILogStatus<string, EParseErrorLevel> destination)
        {
            foreach (var kvp in source.GetLogs())
            {
                destination.AddLog(kvp.Value, kvp.Key);
            }
        }
    }
}
