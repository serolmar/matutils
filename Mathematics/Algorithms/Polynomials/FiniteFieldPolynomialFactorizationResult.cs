namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite representar uma factorização em corpos finitos.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public class FiniteFieldPolynomialFactorizationResult<CoeffType>
    {
        /// <summary>
        /// A lista de factores mónicos.
        /// </summary>
        private List<UnivariatePolynomialNormalForm<CoeffType>> factors;

        /// <summary>
        /// O coeficiente independente - coeficiente principal do polinómio a factorizar.
        /// </summary>
        private CoeffType indepdendentCoeff;

        /// <summary>
        /// O polinómio a ser factorizado.
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> factoredPolynomial;

        public FiniteFieldPolynomialFactorizationResult(
            CoeffType indepdendentCoeff,
            List<UnivariatePolynomialNormalForm<CoeffType>> factors,
            UnivariatePolynomialNormalForm<CoeffType> factoredPolynomial)
        {
            this.factors = factors;
            this.indepdendentCoeff = indepdendentCoeff;
            this.factoredPolynomial = factoredPolynomial;
        }

        /// <summary>
        /// Obtém o coeficiente independente - coeficiente principal do polinómio a factorizar.
        /// </summary>
        public CoeffType IndependentCoeff
        {
            get
            {
                return this.indepdendentCoeff;
            }
        }

        /// <summary>
        /// Obtém a lista de factores mónicos.
        /// </summary>
        public IList<UnivariatePolynomialNormalForm<CoeffType>> Factors
        {
            get
            {
                return this.factors.AsReadOnly();
            }
        }

        /// <summary>
        /// Obtém o polinómio a ser factorizado.
        /// </summary>
        public UnivariatePolynomialNormalForm<CoeffType> FactoredPolynomial
        {
            get
            {
                return this.factoredPolynomial;
            }
        }
    }
}
