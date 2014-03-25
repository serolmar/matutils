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
        private IParse<T, string, string> elementParser;

        private IEuclidenDomain<T> domain;

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
