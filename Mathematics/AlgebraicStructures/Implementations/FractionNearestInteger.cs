namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite determinar o valor inteiro mais próximo de uma fracção.
    /// </summary>
    public class FractionNearestInteger : INearest<Fraction<int, IntegerDomain>, Fraction<int, IntegerDomain>>
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
        public Fraction<int, IntegerDomain> GetNearest(Fraction<int, IntegerDomain> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            else
            {
                var integerPart = source.IntegralPart;
                if (source.IntegralPart == 0)
                {
                    return new Fraction<int, IntegerDomain>(0, 1, this.fractionDomain);
                }
                else
                {
                    var fractionPart = source.FractionalPart;
                    var factor = fractionPart.Denominator / fractionPart.Numerator;
                    if (factor > 2)
                    {
                        ++integerPart;
                    }
                    else if (factor < -2)
                    {
                        --integerPart;
                    }

                    return new Fraction<int,IntegerDomain>(integerPart, 1, this.fractionDomain);
                }
            }
        }
    }
}
