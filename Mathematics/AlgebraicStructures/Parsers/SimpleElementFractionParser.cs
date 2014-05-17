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
        /// Experimenta a leitura de uma fracção simples a partir de uma lista de símbolos.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos.</param>
        /// <param name="value">O valor que receberá a leitura de fracção.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out Fraction<T> value)
        {
            var parsedElement = default(T);
            if (this.elementParser.TryParse(symbolListToParse, out parsedElement))
            {
                value = new Fraction<T>(parsedElement, this.domain.MultiplicativeUnity, this.domain);
                return true;
            }
            else
            {
                value = default(Fraction<T>);
                return false;
            }
        }
    }
}
