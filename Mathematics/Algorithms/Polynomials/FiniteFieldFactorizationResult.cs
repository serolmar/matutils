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
    public class FiniteFieldFactorizationResult<CoeffType>
    {
        /// <summary>
        /// A lista de factores mónicos.
        /// </summary>
        private List<UnivariatePolynomialNormalForm<CoeffType>> factors;

        /// <summary>
        /// O coeficiente independente - coeficiente principal do polinómio a factorizar.
        /// </summary>
        private CoeffType indepdendentCoeff;

        internal FiniteFieldFactorizationResult(
            CoeffType indepdendentCoeff,
            List<UnivariatePolynomialNormalForm<CoeffType>> factors)
        {
            this.factors = factors;
            this.indepdendentCoeff = indepdendentCoeff;
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
    }
}
