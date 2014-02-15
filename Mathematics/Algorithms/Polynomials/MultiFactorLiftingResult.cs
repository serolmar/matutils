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
        /// O conjunto de factores.
        /// </summary>
        private List<UnivariatePolynomialNormalForm<CoeffType>> factors;

        /// <summary>
        /// O corpo modular associado ao número primo original.
        /// </summary>
        private IModularField<CoeffType> modularField;

        /// <summary>
        /// O domínio polinomial baseado no corpo modular.
        /// </summary>
        private UnivarPolynomEuclideanDomain<CoeffType> modularPolynomialDomain;

        /// <summary>
        /// O número primo ao qual os factores foram elevados.
        /// </summary>
        private CoeffType liftingPrimePower;

        internal MultiFactorLiftingResult(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            List<UnivariatePolynomialNormalForm<CoeffType>> factors,
            CoeffType liftingPrimePower,
            IModularField<CoeffType> modularField,
            UnivarPolynomEuclideanDomain<CoeffType> modularPolynomialDomain)
        {
            this.polynom = polynom;
            this.factors = factors;
            this.liftingPrimePower = liftingPrimePower;
            this.modularField = modularField;
            this.modularPolynomialDomain = modularPolynomialDomain;
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
                return this.factors.AsReadOnly();
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

        /// <summary>
        /// Obtém o corpo modular associado ao número primo original.
        /// </summary>
        public IModularField<CoeffType> ModularField
        {
            get
            {
                return this.modularField;
            }
        }
        /// <summary>
        /// Obtém o domínio polinomial baseado no corpo modular.
        /// </summary>
        public UnivarPolynomEuclideanDomain<CoeffType> ModularPolynomialDomain
        {
            get
            {
                return this.modularPolynomialDomain;
            }
        }
    }
}
