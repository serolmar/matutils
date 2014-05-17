namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Aplica o algoritmo de Lagrange para determinar os coeficientes associados à identidade
    /// de Bachet-Bézout.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem os coeficientes.</typeparam>
    public class LagrangeAlgorithm<T> : IBachetBezoutAlgorithm<T>
    {
        /// <summary>
        /// O domínio.
        /// </summary>
        private IEuclidenDomain<T> domain;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LagrangeAlgorithm{T}"/>.
        /// </summary>
        /// <param name="domain">O domónio responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="System.ArgumentNullException">Se o domínio for nulo.</exception>
        public LagrangeAlgorithm(IEuclidenDomain<T> domain)
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
        /// Obtém o domínio inerente ao algoritmo corrente.
        /// </summary>
        /// <value>O domínio.</value>
        public IEuclidenDomain<T> Domain
        {
            get
            {
                return this.domain;
            }
        }

        /// <summary>
        /// Executa o algoritmo sobre dois valores.
        /// </summary>
        /// <param name="first">O primeiro valor.</param>
        /// <param name="second">O segundo valor.</param>
        /// <returns>O resultado composto com os vários parâmetros.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
        public BacheBezoutResult<T> Run(T first, T second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            else
            {
                var prevFirstCoeff = this.domain.MultiplicativeUnity;
                var prevSecondCoeff = this.domain.AdditiveUnity;
                var currentFirstCoeff = this.domain.AdditiveUnity;
                var currentSecondCoeff = this.domain.AdditiveInverse(this.domain.MultiplicativeUnity);
                var prevValue = first;
                var currentValue = second;
                var sign = 1;
                while (!this.domain.IsAdditiveUnity(currentValue))
                {
                    var domainResult = this.domain.GetQuotientAndRemainder(prevValue, currentValue);
                    var firstTempVar = currentFirstCoeff;
                    var secondTempVar = currentSecondCoeff;
                    currentFirstCoeff = this.domain.Multiply(currentFirstCoeff, domainResult.Quotient);
                    currentFirstCoeff = this.domain.Add(currentFirstCoeff, prevFirstCoeff);
                    currentSecondCoeff = this.domain.Multiply(currentSecondCoeff, domainResult.Quotient);
                    currentSecondCoeff = this.domain.Add(currentSecondCoeff, prevSecondCoeff);
                    prevFirstCoeff = firstTempVar;
                    prevSecondCoeff = secondTempVar;
                    prevValue = currentValue;
                    currentValue = domainResult.Remainder;
                    sign = -sign;
                }

                if (sign < 0)
                {
                    prevFirstCoeff = this.domain.AdditiveInverse(prevFirstCoeff);
                    prevSecondCoeff = this.domain.AdditiveInverse(prevSecondCoeff);
                }

                currentSecondCoeff = this.domain.AdditiveInverse(currentSecondCoeff);
                return new BacheBezoutResult<T>(
                    first,
                    second,
                    prevFirstCoeff,
                    prevSecondCoeff,
                    prevValue,
                    currentSecondCoeff,
                    currentFirstCoeff);
            }
        }
    }
}
