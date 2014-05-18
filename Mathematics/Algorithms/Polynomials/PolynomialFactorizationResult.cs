namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um resultado da factorização de um polinómio.
    /// </summary>
    /// <remarks>
    /// É possível que um polinómio com coeficientes fraccionários seja factorizado em vários polinómios
    /// cujos coeficientes sejam inteiros.
    /// </remarks>
    /// <typeparam name="InputPolCoeffType">
    /// O tipo de objectos que constituem os coeficientes do polinómio de entrada.
    /// </typeparam>
    /// <typeparam name="OutputPolCoeffType">
    /// O tipo de objectos que constituem os coeficientes dos factores de saída.
    /// </typeparam>
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

        /// <summary>
        /// Instancia um novo objecto do tipo
        /// <see cref="PolynomialFactorizationResult{InputPolCoeffType, OutputPolCoeffType}"/>.
        /// </summary>
        /// <param name="independentCoeff">O coeficiente independente.</param>
        /// <param name="factors">A lista de factores.</param>
        /// <exception cref="System.ArgumentNullException">Se o coeficiente independente for nulo.</exception>
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
        /// <value>O coeficiente independente.</value>
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
        /// <value>Os factores.</value>
        public Dictionary<int, List<UnivariatePolynomialNormalForm<OutputPolCoeffType>>> Factors
        {
            get
            {
                return this.factors;
            }
        }
    }
}
