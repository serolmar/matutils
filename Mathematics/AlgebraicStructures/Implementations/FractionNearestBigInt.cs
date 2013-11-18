namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Permite determinar o valor inteiro mais próximo de uma fracção.
    /// </summary>
    public class FractionNearestBigInt
        : INearest<Fraction<BigInteger, BigIntegerDomain>, BigInteger>
    {
        /// <summary>
        /// Obtém o valor inteiro mais próximo de uma fracção.
        /// </summary>
        /// <param name="source">A facção.</param>
        /// <returns>O valor inteiro.</returns>
        public BigInteger GetNearest(Fraction<BigInteger, BigIntegerDomain> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            else
            {
                var integerPart = source.IntegralPart;
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

                return integerPart;
            }
        }
    }
}
