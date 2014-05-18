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
        private UnivariatePolynomialNormalForm<CoeffType> factoringPolynomial;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="FiniteFieldPolynomialFactorizationResult{CoeffType}"/>.
        /// </summary>
        /// <param name="indepdendentCoeff">O coeficiente principal do polinómio a factorizar.</param>
        /// <param name="factors">O conjunto de factores.</param>
        /// <param name="factoringPolynomial">O polinómio a ser factorizado.</param>
        public FiniteFieldPolynomialFactorizationResult(
            CoeffType indepdendentCoeff,
            List<UnivariatePolynomialNormalForm<CoeffType>> factors,
            UnivariatePolynomialNormalForm<CoeffType> factoringPolynomial)
        {
            this.factors = factors;
            this.indepdendentCoeff = indepdendentCoeff;
            this.factoringPolynomial = factoringPolynomial;
        }

        /// <summary>
        /// Obtém o coeficiente independente - coeficiente principal do polinómio a factorizar.
        /// </summary>
        /// <value>O coeficiente.</value>
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
        /// <value>
        /// A lista de factores.
        /// </value>
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
        /// <value>O polinómio a ser factorizado.</value>
        public UnivariatePolynomialNormalForm<CoeffType> FactoringPolynomial
        {
            get
            {
                return this.factoringPolynomial;
            }
        }
    }
}
