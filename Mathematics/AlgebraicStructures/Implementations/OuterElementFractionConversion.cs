namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite converter entre fracções e elementos externos.
    /// </summary>
    /// <typeparam name="OutElementType">O tipo do elemento externo.</typeparam>
    /// <typeparam name="FractionElementType">O tipo de elementos que constitui a fracção.</typeparam>
    /// <typeparam name="DomainType">O tipo do domínio que será responsável pelas operações sobre os elementos da fracção.</typeparam>
    public class OuterElementFractionConversion<OutElementType, FractionElementType, DomainType>
        : IConversion<OutElementType, Fraction<FractionElementType, DomainType>>
        where DomainType : IEuclidenDomain<FractionElementType>
    {
        protected DomainType domain;

        protected IConversion<OutElementType, FractionElementType> outTypeToFractionTypeConversion;

        public OuterElementFractionConversion(
            IConversion<OutElementType, FractionElementType> outTypeToFractionTypeConversion,
            DomainType domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (outTypeToFractionTypeConversion == null)
            {
                throw new ArgumentNullException("outTypeToFractionTypeConversion");
            }
            else
            {
                this.domain = domain;
                this.outTypeToFractionTypeConversion = outTypeToFractionTypeConversion;
            }
        }

        public bool CanApplyDirectConversion(Fraction<FractionElementType, DomainType> objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else
            {
                var fractionPartValue = objectToConvert.FractionalPart.Numerator;
                if (this.domain.IsAdditiveUnity(fractionPartValue))
                {
                    return this.outTypeToFractionTypeConversion.CanApplyDirectConversion(
                        objectToConvert.IntegralPart);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CanApplyInverseConversion(OutElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                return true;
            }
            else
            {
                return this.outTypeToFractionTypeConversion.CanApplyInverseConversion(objectToConvert);
            }
        }

        public OutElementType DirectConversion(Fraction<FractionElementType, DomainType> objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                var fractionDecomposition = objectToConvert.FractionDecomposition;
                if (this.domain.IsAdditiveUnity(fractionDecomposition.FractionalPart.Numerator))
                {
                    return this.outTypeToFractionTypeConversion.DirectConversion(fractionDecomposition.IntegralPart);
                }
                else
                {
                    throw new MathematicsException("Can't convert fraction to the matching element. Fraction has fractional part.");
                }
            }
        }

        public Fraction<FractionElementType, DomainType> InverseConversion(OutElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                return new Fraction<FractionElementType, DomainType>(
                    this.outTypeToFractionTypeConversion.InverseConversion(objectToConvert), 
                    this.domain.MultiplicativeUnity, 
                    this.domain);
            }
        }
    }
}
