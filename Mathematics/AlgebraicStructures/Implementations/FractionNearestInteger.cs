namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite determinar o valor inteiro mais próximo de uma fracção.
    /// </summary>
    public class FractionNearestInteger : INearest<Fraction<int>, Fraction<int>>
    {
        private IntegerDomain fractionDomain;

        public FractionNearestInteger(IntegerDomain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                this.fractionDomain = domain;
            }
        }

        /// <summary>
        /// Obtém o valor inteiro mais próximo de uma fracção.
        /// </summary>
        /// <param name="source">A facção.</param>
        /// <returns>O valor inteiro.</returns>
        public Fraction<int> GetNearest(Fraction<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            else
            {
                var integerPart = source.IntegralPart(this.fractionDomain);
                if (integerPart == 0)
                {
                    return new Fraction<int>(0, 1, this.fractionDomain);
                }
                else
                {
                    var fractionPart = source.FractionalPart(this.fractionDomain);
                    var factor = fractionPart.Denominator / fractionPart.Numerator;
                    if (factor > 2)
                    {
                        ++integerPart;
                    }
                    else if (factor < -2)
                    {
                        --integerPart;
                    }

                    return new Fraction<int>(integerPart, 1, this.fractionDomain);
                }
            }
        }
    }
}
