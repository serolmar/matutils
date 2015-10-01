namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Representa um leitor de itens como sendo fracções.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de objectos que constituem os coeficientes das fracções.</typeparam>
    public class ElementFractionParser<ElementType> : IParse<Fraction<ElementType>, string, string>
    {
        /// <summary>
        /// O domínio euclideano associado à fracção.
        /// </summary>
        private IEuclidenDomain<ElementType> domain;

        /// <summary>
        /// O leitor para o objecto.
        /// </summary>
        protected IParse<ElementType, string, string> elementParser;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ElementFractionParser{ElementType}"/>.
        /// </summary>
        /// <param name="elementParser">O leitor de coficientes.</param>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        public ElementFractionParser(
            IParse<ElementType, string, string> elementParser, 
            IEuclidenDomain<ElementType> domain)
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
        /// Otém o domínio responsável pelas operações sobre os coeficientes.
        /// </summary>
        /// <value>O domínio responsável pelas operações sobre os coeficientes.</value>
        public IEuclidenDomain<ElementType> Domain
        {
            get
            {
                return this.domain;
            }
        }

        /// <summary>
        /// Obtém o leitor dos objectos.
        /// </summary>
        /// <value>O leitor dos objectos.</value>
        public IParse<ElementType, string, string> SimpleObjectParser
        {
            get
            {
                return this.elementParser;
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
        public Fraction<ElementType> Parse(
            ISymbol<string, string>[] symbolListToParse, 
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var innerErrorLog = new LogStatus<string, EParseErrorLevel>();
            var parsed = this.elementParser.Parse(symbolListToParse, innerErrorLog);
            if (innerErrorLog.HasLogs(EParseErrorLevel.ERROR))
            {
                foreach (var message in innerErrorLog.GetLogs(EParseErrorLevel.ERROR))
                {
                    errorLogs.AddLog(message, EParseErrorLevel.ERROR);
                }

                return default(Fraction<ElementType>);
            }
            else
            {
                return new Fraction<ElementType>(parsed, this.domain.MultiplicativeUnity, this.domain);
            }
        }
    }
}
