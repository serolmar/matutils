namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CoeffFractionMultiplicationOperation<CoeffType, DomainType>
        : IMultiplicationOperation<CoeffType, Fraction<CoeffType,DomainType>, Fraction<CoeffType,DomainType>>
        where DomainType : IEuclidenDomain<CoeffType>
    {
        /// <summary>
        /// Permite obter a multiplicação do coeficiente por uma fracção.
        /// </summary>
        /// <param name="left">O coeficiente.</param>
        /// <param name="right">A fracção.</param>
        /// <returns>A fracção resultante do produto do coeficiente pela fracção.</returns>
        public Fraction<CoeffType, DomainType> Multiply(
            CoeffType left, 
            Fraction<CoeffType, DomainType> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var numerator = right.Domain.Multiply(left, right.Numerator);
                if (right.Domain.IsAdditiveUnity(numerator))
                {
                    return new Fraction<CoeffType, DomainType>(right.Domain);
                }
                else
                {
                    return new Fraction<CoeffType, DomainType>(
                        numerator,
                        right.Denominator,
                        right.Domain);
                }
            }
        }
    }
}
