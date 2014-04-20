namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Algoritmo que permite determinar uma factorização racional de um polinómio.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public class IntegerPolynomialFactorizationAlgorithm<CoeffType>
        : IAlgorithm<UnivariatePolynomialNormalForm<Fraction<CoeffType>>,
        PolynomialFactorizationResult<Fraction<CoeffType>, CoeffType>>
    {
        /// <summary>
        /// O objecto responsável pelas operações sobre os números inteiros.
        /// </summary>
        private IIntegerNumber<CoeffType> integerNumber;

        /// <summary>
        /// Permite criar instâncias de objectos responsáveis pelas operações modulares.
        /// </summary>
        IModularFieldFactory<CoeffType> modularSymmetricFactory;

        /// <summary>
        /// O algoritmo que permite factoizar um polinómio em factores livres de quadrados.
        /// </summary>
        private IAlgorithm<UnivariatePolynomialNormalForm<Fraction<CoeffType>>,
            SquareFreeFactorizationResult<Fraction<CoeffType>, CoeffType>> squareFreeAlg;

        /// <summary>
        /// Permite aplicar o método da pesquisa até um número pré-definido de níveis.
        /// </summary>
        private IAlgorithm<MultiFactorLiftingResult<CoeffType>, CoeffType, int,
            SearchFactorizationResult<CoeffType>> searchFactorizationAlgorithm;

        public IntegerPolynomialFactorizationAlgorithm(
            IIntegerNumber<CoeffType> integerNumber,
            IModularFieldFactory<CoeffType> modularSymmetricFactory)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (modularSymmetricFactory == null)
            {
                throw new ArgumentNullException("modularSymmetricFactory");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.modularSymmetricFactory = modularSymmetricFactory;
                this.squareFreeAlg = new SquareFreeFractionFactorizationAlg<CoeffType>(integerNumber);
                this.searchFactorizationAlgorithm = new SearchFactorizationAlgorithm<CoeffType>(
                    modularSymmetricFactory,
                    this.integerNumber);
            }
        }

        /// <summary>
        /// Permite determinar a factorização racional de um polinómio.
        /// </summary>
        /// <param name="polynomial">O polinómio.</param>
        /// <returns>O resultado da factorização.</returns>
        public PolynomialFactorizationResult<Fraction<CoeffType>, CoeffType> Run(
            UnivariatePolynomialNormalForm<Fraction<CoeffType>> polynomial)
        {
            if (polynomial == null)
            {
                throw new ArgumentNullException("polynomial");
            }
            else
            {
                var squareFreeResult = this.squareFreeAlg.Run(polynomial);
                var independentCoeff = squareFreeResult.IndependentCoeff;
                var factorizationDictionary =
                    new Dictionary<int, List<UnivariatePolynomialNormalForm<CoeffType>>>();
                foreach (var squareFreeFactorKvp in squareFreeResult.Factors)
                {
                    if (squareFreeFactorKvp.Value.Degree == 1)
                    {
                        factorizationDictionary.Add(
                            squareFreeFactorKvp.Key,
                            new List<UnivariatePolynomialNormalForm<CoeffType>>() { squareFreeFactorKvp.Value });
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                return new PolynomialFactorizationResult<Fraction<CoeffType>, CoeffType>(
                    independentCoeff,
                    factorizationDictionary);
            }
        }

        private CoeffType ComputeApproximation(
            CoeffType polynomialNorm,
            CoeffType leadingCoeff,
            int degree)
        {
            var result = MathFunctions.Power(this.integerNumber.MapFrom(2), degree, this.integerNumber);
            result = this.integerNumber.Multiply(result, leadingCoeff);
            result = this.integerNumber.Multiply(result, polynomialNorm);

            // Utilização da expansão em série para a determinação do valor inteiro da raiz quadrada.
            // a_1=1, b_1=1, a_{n+1}=-(2n+1)a_n, b_{n+1}=2(n+1)b_n
            throw new NotImplementedException();
        }
    }
}
