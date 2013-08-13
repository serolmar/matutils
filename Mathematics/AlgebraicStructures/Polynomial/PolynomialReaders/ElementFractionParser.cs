namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    /// <summary>
    /// Representa um leitor de itens como sendo fracções.
    /// </summary>
    public class ElementFractionParser<ElementType, DomainType> : IParse<Fraction<ElementType, DomainType>, string, string>
        where DomainType : IEuclidenDomain<ElementType>
    {
        private DomainType domain;

        protected IParse<ElementType, string, string> elementParser;

        public ElementFractionParser(IParse<ElementType, string, string> elementParser, DomainType domain)
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
            }
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out Fraction<ElementType, DomainType> value)
        {
            value = null;
            var parsedElement = default(ElementType);
            if (this.elementParser.TryParse(symbolListToParse, out parsedElement))
            {
                value = new Fraction<ElementType, DomainType>(parsedElement, this.domain.MultiplicativeUnity, this.domain);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
