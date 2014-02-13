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

        internal SearchFactorizationResult(
            UnivariatePolynomialNormalForm<CoeffType> mainPolynomial,
            List<UnivariatePolynomialNormalForm<CoeffType>> integerFactors)
        {
            this.mainPolynomial = mainPolynomial;
            this.integerFactors = integerFactors;
        }

        /// <summary>
        /// O polinómio do qual se procurar uma factorização.
        /// </summary>
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
        public IList<UnivariatePolynomialNormalForm<CoeffType>> NonIntegerFactors
        {
            get
            {
                return this.nonIntegerFactors.AsReadOnly();
            }
        }
    }
}
