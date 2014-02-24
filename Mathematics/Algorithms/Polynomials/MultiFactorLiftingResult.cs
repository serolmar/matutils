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
        /// O conjunto de factores mónicos.
        /// </summary>
        private IList<UnivariatePolynomialNormalForm<CoeffType>> factors;

        /// <summary>
        /// O número primo ao qual os factores foram elevados.
        /// </summary>
        private CoeffType liftingPrimePower;

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
        public UnivariatePolynomialNormalForm<CoeffType> Polynom
        {
            get
            {
                return this.polynom;
            }
        }

        /// <summary>
        /// Obtém o conjunto de factores.
        /// </summary>
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
        public CoeffType LiftingPrimePower
        {
            get
            {
                return this.liftingPrimePower;
            }
        }
    }
}
