// -----------------------------------------------------------------------
// <copyright file="ElementFractionConversion.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Converte entre o elemento e a respectiva fracção.
    /// </summary>
    public class ElementFractionConversion<ElementType, DomainType> : IConversion<ElementType, Fraction<ElementType, DomainType>>
        where DomainType : IEuclidenDomain<ElementType>
    {
        protected DomainType domain;

        public ElementFractionConversion(DomainType domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                this.domain = domain;
            }
        }

        public DomainType Domain {
            get
            {
                return this.domain;
            }
        }

        public bool CanApplyDirectConversion(Fraction<ElementType, DomainType> objectToConvert)
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CanApplyInverseConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ElementType DirectConversion(Fraction<ElementType, DomainType> objectToConvert)
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
                    return fractionDecomposition.IntegralPart;
                }
                else
                {
                    throw new MathematicsException("Can't convert fraction to the matching element. Fraction has fractional part.");
                }
            }
        }

        public Fraction<ElementType, DomainType> InverseConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                return new Fraction<ElementType, DomainType>(objectToConvert, this.domain.MultiplicativeUnity, this.domain);
            }
        }
    }
}
