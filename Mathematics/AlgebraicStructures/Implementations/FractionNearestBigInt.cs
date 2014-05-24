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
        : INearest<Fraction<BigInteger>, BigInteger>
    {
        /// <summary>
        /// O domínio responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IEuclidenDomain<BigInteger> domain;

        /// <summary>
        /// Cria uma instância de objectos do tipo <see cref="FractionNearestBigInt"/>.
        /// </summary>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">Caso o domínio seja nulo.</exception>
        public FractionNearestBigInt(IEuclidenDomain<BigInteger> domain)
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
        /// Obtém o valor inteiro mais próximo de uma fracção.
        /// </summary>
        /// <param name="source">A facção.</param>
        /// <returns>O valor inteiro.</returns>
        /// <exception cref="ArgumentNullException">Caso o argumento seja nulo.</exception>
        public BigInteger GetNearest(Fraction<BigInteger> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            else
            {
                var integerPart = source.IntegralPart(this.domain);
                var fractionPart = source.FractionalPart(this.domain);
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
