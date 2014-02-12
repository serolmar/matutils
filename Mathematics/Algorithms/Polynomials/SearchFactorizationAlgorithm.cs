namespace Mathematics.Algorithms.Polynomials
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite aplicar o método da pesquisa a uma factorização módulo um número primo elevado
    /// de modo a torná-la numa factorização sobre os inteiros.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de dados dos coeficientes.</typeparam>
    public class SearchFactorizationAlgorithm<CoeffType>
        : IAlgorithm<MultiFactorLiftingResult<CoeffType>, CoeffType, int, List<UnivariatePolynomialNormalForm<CoeffType>>>
    {
        IIntegerNumber<CoeffType> integerNumber;

        public SearchFactorizationAlgorithm(IIntegerNumber<CoeffType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else
            {
                this.integerNumber = integerNumber;
            }
        }

        /// <summary>
        /// Permite obter a lista de factores irredutíveis a partir de uma factorização módulo um número primo.
        /// </summary>
        /// <param name="liftedFactorization">A factorização módulo o número primo.</param>
        /// <param name="testValue">
        /// O valor de teste que depende do grau, da norma e do coeficiente principal do polinómio original.
        /// </param>
        /// <param name="combinationsNumber">O número máximo de combinações a testar.</param>
        /// <returns>A lista de factores irredutíveis.</returns>
        public List<UnivariatePolynomialNormalForm<CoeffType>> Run(
            MultiFactorLiftingResult<CoeffType> liftedFactorization,
            CoeffType testValue,
            int combinationsNumber)
        {
            if (liftedFactorization == null)
            {
                throw new ArgumentNullException("liftedFactorization");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Permite obter a norma de um polinómio.
        /// </summary>
        /// <param name="polynom">O polinómio.</param>
        /// <returns>A norma do polinómio.</returns>
        private CoeffType GetPlynomialNorm(UnivariatePolynomialNormalForm<CoeffType> polynom)
        {
            var result = this.integerNumber.AdditiveUnity;
            foreach (var term in polynom)
            {
                var termNorm = this.integerNumber.GetNorm(term.Value);
                if (this.integerNumber.Compare(termNorm, result) > 0)
                {
                    result = termNorm;
                }
            }

            return result;
        }
    }
}
