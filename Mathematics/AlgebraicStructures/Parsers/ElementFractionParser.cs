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
        /// Experimenta a leitura da fracção.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos a partir da qual a leitura é realizada.</param>
        /// <param name="value">Recebe o resultado da leitura.</param>
        /// <returns>Veradeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out Fraction<ElementType> value)
        {
            value = null;
            var parsedElement = default(ElementType);
            if (this.elementParser.TryParse(symbolListToParse, out parsedElement))
            {
                value = new Fraction<ElementType>(parsedElement, this.domain.MultiplicativeUnity, this.domain);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
