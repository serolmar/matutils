namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CoeffFractionMultiplicationOperation<CoeffType>
        : IMultiplicationOperation<CoeffType, Fraction<CoeffType>, Fraction<CoeffType>>
    {
        /// <summary>
        /// O domínio responsável pelas operações de simplificação sobre os coeficientes.
        /// </summary>
        private IEuclidenDomain<CoeffType> domain;

        public CoeffFractionMultiplicationOperation(IEuclidenDomain<CoeffType> domain)
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

        /// <summary>
        /// Permite obter a multiplicação do coeficiente por uma fracção.
        /// </summary>
        /// <param name="left">O coeficiente.</param>
        /// <param name="right">A fracção.</param>
        /// <returns>A fracção resultante do produto do coeficiente pela fracção.</returns>
        public Fraction<CoeffType> Multiply(
            CoeffType left, 
            Fraction<CoeffType> right)
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
                var numerator = this.domain.Multiply(left, right.Numerator);
                if (this.domain.IsAdditiveUnity(numerator))
                {
                    return new Fraction<CoeffType>(this.domain);
                }
                else
                {
                    return new Fraction<CoeffType>(
                        numerator,
                        right.Denominator,
                        this.domain);
                }
            }
        }
    }
}
