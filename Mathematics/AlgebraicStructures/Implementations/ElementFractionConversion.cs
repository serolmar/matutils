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
    using Utilities;

    /// <summary>
    /// Converte entre o elemento e a respectiva fracção.
    /// </summary>
    public class ElementFractionConversion<ElementType> 
        : IConversion<ElementType, Fraction<ElementType>>
    {
        protected IEuclidenDomain<ElementType> domain;

        public ElementFractionConversion(IEuclidenDomain<ElementType> domain)
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

        public IEuclidenDomain<ElementType> Domain
        {
            get
            {
                return this.domain;
            }
        }

        public bool CanApplyDirectConversion(Fraction<ElementType> objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else
            {
                var fractionPartValue = objectToConvert.FractionalPart(this.domain).Numerator;
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
                return false;
            }
            else
            {
                return true; ;
            }
        }

        public ElementType DirectConversion(Fraction<ElementType> objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                var fractionDecomposition = objectToConvert.FractionDecomposition(this.domain);
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

        public Fraction<ElementType> InverseConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                return new Fraction<ElementType>(objectToConvert, this.domain.MultiplicativeUnity, this.domain);
            }
        }
    }
}
