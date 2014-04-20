namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PolynomialFactorizationResult<InputPolCoeffType, OutputPolCoeffType>
    {
        /// <summary>
        /// O coeficiente independente.
        /// </summary>
        private InputPolCoeffType independentCoeff;

        /// <summary>
        /// Os facotres seleccionados por grau.
        /// </summary>
        private Dictionary<int, List<UnivariatePolynomialNormalForm<OutputPolCoeffType>>> factors;

        public PolynomialFactorizationResult(
            InputPolCoeffType independentCoeff,
            Dictionary<int, List<UnivariatePolynomialNormalForm<OutputPolCoeffType>>> factors)
        {
            if (independentCoeff == null)
            {
                throw new ArgumentNullException("independentCoeff");
            }
            else if (factors == null)
            {
                this.independentCoeff = independentCoeff;
                this.factors = new Dictionary<int, List<UnivariatePolynomialNormalForm<OutputPolCoeffType>>>();
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
        public Dictionary<int, List<UnivariatePolynomialNormalForm<OutputPolCoeffType>>> Factors
        {
            get
            {
                return this.factors;
            }
        }
    }
}
