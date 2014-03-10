namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa o resultado de uma factorização livre de quadrados quando existe diferença entre
    /// o tipo de dados do polinómio de entrada e os factores de saída.
    /// </summary>
    /// <typeparam name="InputPolCoeffType">O tipo de dados do coeficientes do polinómio de entrada.</typeparam>
    /// <typeparam name="OutputPolCoeffType">O tipo de dados dos factores de saída.</typeparam>
    public class SquareFreeFactorizationResult<InputPolCoeffType, OutputPolCoeffType>
    {
        /// <summary>
        /// O coeficiente independente.
        /// </summary>
        private InputPolCoeffType independentCoeff;

        /// <summary>
        /// Os facotres seleccionados por grau.
        /// </summary>
        private Dictionary<int, UnivariatePolynomialNormalForm<OutputPolCoeffType>> factors;

        public SquareFreeFactorizationResult(
            InputPolCoeffType independentCoeff,
            Dictionary<int, UnivariatePolynomialNormalForm<OutputPolCoeffType>> factors)
        {
            if (independentCoeff == null)
            {
                throw new ArgumentNullException("independentCoeff");
            }
            else if (factors == null)
            {
                this.independentCoeff = independentCoeff;
                this.factors = new Dictionary<int, UnivariatePolynomialNormalForm<OutputPolCoeffType>>();
            }
            else
            {
                this.independentCoeff = independentCoeff;
                this.factors = factors;
            }
        }

        /// <summary>
        /// Obtém o coeficiente independente.
        /// </summary>
        public InputPolCoeffType IndependentCoeff
        {
            get
            {
                return this.independentCoeff;
            }
        }

        /// <summary>
        /// Obtém os factores seleccionados por grau.
        /// </summary>
        public Dictionary<int, UnivariatePolynomialNormalForm<OutputPolCoeffType>> Factors
        {
            get
            {
                return this.factors;
            }
        }
    }
}
