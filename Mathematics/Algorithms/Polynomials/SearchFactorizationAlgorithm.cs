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
        : IAlgorithm<MultiFactorLiftingResult<CoeffType>, CoeffType, int, SearchFactorizationResult<CoeffType>>
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
        public SearchFactorizationResult<CoeffType> Run(
            MultiFactorLiftingResult<CoeffType> liftedFactorization,
            CoeffType testValue,
            int combinationsNumber)
        {
            if (liftedFactorization == null)
            {
                throw new ArgumentNullException("liftedFactorization");
            }
            else if (combinationsNumber <= 0)
            {
                throw new ArgumentException("At least on combination is expected.");
            }
            else
            {
                var halfPrime = this.integerNumber.Quo(
                    liftedFactorization.LiftingPrime,
                    this.integerNumber.MapFrom(2));

                var modularFactors = new List<UnivariatePolynomialNormalForm<CoeffType>>();
                foreach (var liftedFactor in liftedFactorization.Factors)
                {
                    if (liftedFactor.Degree != 0)
                    {
                        modularFactors.Add(liftedFactor.ApplyFunction(
                            c => this.GetSymmetricRemainder(c, liftedFactorization.LiftingPrime, halfPrime),
                            this.integerNumber));
                    }
                }

                var integerFactors = new List<UnivariatePolynomialNormalForm<CoeffType>>();

                // Lida com os factores lineares
                var i = 0;
                var constantCoeff = this.integerNumber.MultiplicativeUnity;
                while (i < modularFactors.Count)
                {
                    var currentModularFactor = modularFactors[i];
                    var norm = this.GetPlynomialNorm(currentModularFactor);
                    if (this.integerNumber.Compare(norm, testValue) < 0)
                    {
                        integerFactors.Add(currentModularFactor);
                        modularFactors.RemoveAt(i);
                    }
                    else
                    {
                        ++i;
                    }
                }

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

        /// <summary>
        /// Obtém a representação do número modular entre 0 e p num sistema que varia de -p/2 a p/2.
        /// </summary>
        /// <param name="coeff">O coeficiente modular na representação entre 0 e p.</param>
        /// <param name="prime">O número primo.</param>
        /// <param name="halfPrime">A metade do número primo.</param>
        /// <returns>A representação no sistema que varia de -p/2 a p/2.</returns>
        private CoeffType GetSymmetricRemainder(CoeffType coeff, CoeffType prime, CoeffType halfPrime)
        {
            var result = coeff;
            if (this.integerNumber.Compare(halfPrime, coeff) < 0)
            {
                result = this.integerNumber.Add(coeff, this.integerNumber.AdditiveInverse(prime));
            }

            return result;
        }
    }
}
