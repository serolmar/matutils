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
    public class ElementFractionParser<ElementType> : IParse<Fraction<ElementType>, string, string>
    {
        private IEuclidenDomain<ElementType> domain;

        protected IParse<ElementType, string, string> elementParser;

        public ElementFractionParser(IParse<ElementType, string, string> elementParser, IEuclidenDomain<ElementType> domain)
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
