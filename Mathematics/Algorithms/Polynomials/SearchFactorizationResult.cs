namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite manter o resultado da factorização por pesquisa.
    /// </summary>
    /// <remarks>
    /// Uma vez  que o algoritmo de pesquisa pode ser configurado para executar sobre um número pre-definido
    /// de factores, é necessário guardar o factor do polinómio que foi decomposto em subfactores inteiros
    /// e aquele que é candidato a factor irredutível mas contendo uma factorização modular.
    /// </remarks>
    /// <typeparam name="CoeffType">O tipo de coeficientes.</typeparam>
    public class SearchFactorizationResult<CoeffType>
    {
        /// <summary>
        /// O polinómio do qual se procurar uma factorização.
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> mainPolynomial;

        /// <summary>
        /// O conjunto de subfactores inteiros do factor para o qual se conhece a respectiva factorização.
        /// </summary>
        private List<UnivariatePolynomialNormalForm<CoeffType>> integerFactors;

        /// <summary>
        /// O conjunto de subfactores modulares do factor que não foi decomposto.
        /// </summary>
        private List<UnivariatePolynomialNormalForm<CoeffType>> nonIntegerFactors;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SearchFactorizationResult{CoeffType}"/>.
        /// </summary>
        /// <param name="mainPolynomial">O polinómio principal.</param>
        /// <param name="integerFactors">O conjunto de subfactores com coeficientes inteiros.</param>
        /// <param name="nonIntegerFactors">O conjunto de factores com coeficientes não inteiros.</param>
        internal SearchFactorizationResult(
            UnivariatePolynomialNormalForm<CoeffType> mainPolynomial,
            List<UnivariatePolynomialNormalForm<CoeffType>> integerFactors,
            List<UnivariatePolynomialNormalForm<CoeffType>> nonIntegerFactors)
        {
            this.mainPolynomial = mainPolynomial;
            this.integerFactors = integerFactors;
            this.nonIntegerFactors = nonIntegerFactors;
        }

        /// <summary>
        /// O polinómio do qual se procurar uma factorização.
        /// </summary>
        /// <value>O polinómio.</value>
        public UnivariatePolynomialNormalForm<CoeffType> MainPolynomial
        {
            get
            {
                return this.mainPolynomial;
            }
        }

        /// <summary>
        /// O conjunto de subfactores inteiros do factor para o qual se conhece a respectiva factorização.
        /// </summary>
        /// <value>Os factores com coeficientes inteiros.</value>
        public IList<UnivariatePolynomialNormalForm<CoeffType>> IntegerFactors
        {
            get
            {
                return this.integerFactors.AsReadOnly();
            }
        }

        /// <summary>
        /// O conjunto de subfactores modulares do factor que não foi decomposto.
        /// </summary>
        /// <value>O conjunto de factores com coeficientes não inteiros.</value>
        public IList<UnivariatePolynomialNormalForm<CoeffType>> NonIntegerFactors
        {
            get
            {
                return this.nonIntegerFactors.AsReadOnly();
            }
        }
    }
}
