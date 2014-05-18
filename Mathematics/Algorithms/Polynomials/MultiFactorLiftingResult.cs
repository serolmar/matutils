namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Condensa o resultado de um levantamento multifactor.
    /// </summary>
    public class MultiFactorLiftingResult<CoeffType>
    {
        /// <summary>
        /// O polinómio.
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> polynom;

        /// <summary>
        /// O conjunto de factores mónicos com excepção do primeiro.
        /// </summary>
        private IList<UnivariatePolynomialNormalForm<CoeffType>> factors;

        /// <summary>
        /// O número primo ao qual os factores foram elevados.
        /// </summary>
        private CoeffType liftingPrimePower;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="MultiFactorLiftingResult{CoeffType}"/>.
        /// </summary>
        /// <param name="polynom">O polinómio.</param>
        /// <param name="factors">O conjunto de factores.</param>
        /// <param name="liftingPrimePower">O primo módulo o qual os factores foram elevados.</param>
        internal MultiFactorLiftingResult(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            IList<UnivariatePolynomialNormalForm<CoeffType>> factors,
            CoeffType liftingPrimePower)
        {
            this.polynom = polynom;
            this.factors = factors;
            this.liftingPrimePower = liftingPrimePower;
        }

        /// <summary>
        /// Obtém o polinómio.
        /// </summary>
        /// <value>O polinómio.</value>
        public UnivariatePolynomialNormalForm<CoeffType> Polynom
        {
            get
            {
                return this.polynom;
            }
        }

        /// <summary>
        /// Obtém o conjunto de factores sendo todos mónicos com excepção de, possivelmente, o primeiro.
        /// </summary>
        /// <value>O conjunto de factores.</value>
        public IList<UnivariatePolynomialNormalForm<CoeffType>> Factors
        {
            get
            {
                return this.factors;
            }
        }

        /// <summary>
        /// Obtém o primo módulo o qual os factores foram elevados.
        /// </summary>
        /// <value>O primo.</value>
        public CoeffType LiftingPrimePower
        {
            get
            {
                return this.liftingPrimePower;
            }
        }
    }
}
