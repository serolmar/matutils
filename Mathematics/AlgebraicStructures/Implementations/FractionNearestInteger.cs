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
        /// <summary>
        /// O domínio responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IntegerDomain fractionDomain;

        /// <summary>
        /// Cria uma instância de objectos do tipo <see cref="FractionNearestInteger"/>.
        /// </summary>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="System.ArgumentNullException">CAso o domínio seja nulo.</exception>
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
        /// <exception cref="ArgumentNullException">Caso o argumento seja nulo.</exception>
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
