namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Permite realizar a leitura de um elemento simples como sendo uma fracção.
    /// </summary>
    /// <typeparam name="T">O tipo de dados associado ao elemento.</typeparam>
    public class SimpleElementFractionParser<T> : IParse<Fraction<T>, string, string>
    {
        /// <summary>
        /// O leitor de coeficientes.
        /// </summary>
        private IParse<T, string, string> elementParser;

        /// <summary>
        /// O domínio responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IEuclidenDomain<T> domain;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimpleElementFractionParser{T}"/>.
        /// </summary>
        /// <param name="elementParser">O leitor de coeficientes.</param>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        public SimpleElementFractionParser(
            IParse<T, string, string> elementParser,
            IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (elementParser == null)
            {
                throw new ArgumentNullException("elementParser");
            }
            else
            {
                this.elementParser = elementParser;
                this.domain = domain;
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
        public Fraction<T> Parse(
            ISymbol<string, string>[] symbolListToParse, 
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var innerErrors = new LogStatus<string, EParseErrorLevel>();
            var parsedElement = this.elementParser.Parse(symbolListToParse, innerErrors);
            foreach (var kvp in innerErrors.GetLogs())
            {
                errorLogs.AddLog(kvp.Value, kvp.Key);
            }

            if (innerErrors.HasLogs(EParseErrorLevel.ERROR))
            {
                return default(Fraction<T>);
            }
            else
            {
                return new Fraction<T>(parsedElement, this.domain.MultiplicativeUnity, this.domain);
            }
        }
    }
}
