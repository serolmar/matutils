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
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os coeficientes dos polinómios.</typeparam>
    internal class SimplePolynomialReader<CoeffType> :
        IParse<ParsePolynomialItem<CoeffType>, string, string>
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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimplePolynomialReader{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffParser">O leitor de coeficientes.</param>
        /// <param name="coeffRing">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="MathematicsException">Se o leitor de coeficientes não for fornecido.</exception>
        public SimplePolynomialReader(IParse<CoeffType, string, string> coeffParser, IRing<CoeffType> coeffRing)
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
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public ParsePolynomialItem<CoeffType> Parse(
            ISymbol<string, string>[] symbolListToParse, 
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var pol = default(ParsePolynomialItem<CoeffType>);
            var innerError = new LogStatus<string,EParseErrorLevel>();
            var parsedCoeff = this.coeffParser.Parse(symbolListToParse, innerError);
            if (innerError.HasLogs(EParseErrorLevel.ERROR))
            {
                if (symbolListToParse.Length == 1)
                {
                    var stringValue = symbolListToParse[0].SymbolValue;
                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        errorLogs.AddLog(
                            "Variable name can't be empty.",
                            EParseErrorLevel.ERROR);
                    }
                    else if (char.IsLetter(stringValue[0]))
                    {
                        pol = new ParsePolynomialItem<CoeffType>();
                        pol.Polynomial = new Polynomial<CoeffType>(this.coeffRing.MultiplicativeUnity, stringValue, this.coeffRing);
                    }
                    else
                    {
                        var degreeError = new LogStatus<string, EParseErrorLevel>();
                        var integerValue = this.integerParser.Parse(symbolListToParse, degreeError);
                        if (!degreeError.HasLogs(EParseErrorLevel.ERROR))
                        {
                            pol = new ParsePolynomialItem<CoeffType>();
                            pol.Degree = integerValue;
                        }

                        this.CopyErrors(degreeError, errorLogs);
                    }
                }
                else
                {
                    var degreeError = new LogStatus<string, EParseErrorLevel>();
                    var integerValue = this.integerParser.Parse(symbolListToParse, degreeError);
                    if (!degreeError.HasLogs(EParseErrorLevel.ERROR))
                    {
                        pol = new ParsePolynomialItem<CoeffType>();
                        pol.Degree = integerValue;
                    }

                    this.CopyErrors(degreeError, errorLogs);
                }
            }
            else
            {
                pol = new ParsePolynomialItem<CoeffType>();
                pol.Coeff = parsedCoeff;

                // Poderão existir avisos
                this.CopyErrors(innerError, errorLogs);
            }

            return pol;
        }

        /// <summary>
        /// Copia mensagens entre diários.
        /// </summary>
        /// <param name="source">O diário de origem.</param>
        /// <param name="destination">O diário de destino.</param>
        private void CopyErrors(
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
