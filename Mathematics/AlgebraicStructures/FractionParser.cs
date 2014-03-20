namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    internal class FractionParser<ObjectType> : IParse<Fraction<ObjectType>, string, string>
    {
        /// <summary>
        /// O domínio euclideano associado à fracção.
        /// </summary>
        private IEuclidenDomain<ObjectType> domain;

        /// <summary>
        /// O leitor para o objecto.
        /// </summary>
        private IParse<ObjectType, string, string> simpleObjectParser;

        public FractionParser(IParse<ObjectType, string, string> simpleObjectParser, IEuclidenDomain<ObjectType> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (simpleObjectParser == null)
            {
                throw new ArgumentNullException("objectParser");
            }
            else
            {
                this.domain = domain;
                this.simpleObjectParser = simpleObjectParser;
            }
        }

        public IEuclidenDomain<ObjectType> Domain
        {
            get
            {
                return this.domain;
            }
        }

        public IParse<ObjectType, string, string> SimpleObjectParser
        {
            get
            {
                return this.simpleObjectParser;
            }
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out Fraction<ObjectType> value)
        {
            value = null;
            var readedObject = default(ObjectType);
            if (this.simpleObjectParser.TryParse(symbolListToParse, out readedObject))
            {
                value = new Fraction<ObjectType>(readedObject,
                    this.domain.MultiplicativeUnity,
                    this.domain);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
