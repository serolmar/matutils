namespace Mathematics
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
        /// <summary>
        /// Responsável pelas operações sobre os números inteiros.
        /// </summary>
        private IIntegerNumber<CoeffType> integerNumber;

        /// <summary>
        /// Responsável pela instanciação de corpos modulares.
        /// </summary>
        private IModularFieldFactory<CoeffType> modularFieldFactory;

        public SearchFactorizationAlgorithm(
            IModularFieldFactory<CoeffType> modularFieldFactory,
            IIntegerNumber<CoeffType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (modularFieldFactory == null)
            {
                throw new ArgumentNullException("modularFieldFactory");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.modularFieldFactory = modularFieldFactory;
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
                throw new ArgumentException("At least one combination is expected.");
            }
            else
            {
                var modularField = this.modularFieldFactory.CreateInstance(liftedFactorization.LiftingPrimePower);
                var halfPrime = this.integerNumber.Quo(
                    liftedFactorization.LiftingPrimePower,
                    this.integerNumber.MapFrom(2));

                var modularFactors = new List<UnivariatePolynomialNormalForm<CoeffType>>();
                foreach (var liftedFactor in liftedFactorization.Factors)
                {
                    if (liftedFactor.Degree != 0)
                    {
                        modularFactors.Add(liftedFactor.ApplyFunction(
                            c => this.GetSymmetricRemainder(c, liftedFactorization.LiftingPrimePower, halfPrime),
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

                var modularPolynomialDomain = new UnivarPolynomEuclideanDomain<CoeffType>(
                    liftedFactorization.Polynom.VariableName,
                    modularField);
                this.ProcessRemainingPolynomials(
                    modularFactors,
                    integerFactors,
                    liftedFactorization.LiftingPrimePower,
                    halfPrime,
                    testValue,
                    combinationsNumber,
                    modularPolynomialDomain);

                return new SearchFactorizationResult<CoeffType>(
                    liftedFactorization.Polynom,
                    integerFactors,
                    modularFactors);
            }
        }

        /// <summary>
        /// Permite processar as restantes combinações.
        /// </summary>
        /// <param name="modularFactors">Os factores modulares.</param>
        /// <param name="integerFactors">Os factores inteiros.</param>
        /// <param name="primePower">A potência de um número primo que servirá de módulo.</param>
        /// <param name="halfPrimePower">A metade da potência do número primo.</param>
        /// <param name="testValue">O valor de teste.</param>
        /// <param name="combinationsNumber">O número máximo de polinómios nas combinações.</param>
        /// <param name="modularPolynomialDomain">O corpo responsável pelas operações modulares.</param>
        private void ProcessRemainingPolynomials(
            List<UnivariatePolynomialNormalForm<CoeffType>> modularFactors,
            List<UnivariatePolynomialNormalForm<CoeffType>> integerFactors,
            CoeffType primePower,
            CoeffType halfPrimePower,
            CoeffType testValue,
            int combinationsNumber,
            UnivarPolynomEuclideanDomain<CoeffType> modularPolynomialDomain)
        {
            if (combinationsNumber > 1 && modularFactors.Count > 1)
            {
                var modularProduct = default(UnivariatePolynomialNormalForm<CoeffType>);
                var currentCombinationNumber = 2;
                var productStack = new Stack<UnivariatePolynomialNormalForm<CoeffType>>();
                productStack.Push(modularFactors[0]);
                var pointers = new Stack<int>();
                pointers.Push(0);
                pointers.Push(1);
                var state = 0;
                while (state != -1)
                {
                    if (state == 0)
                    {
                        if (pointers.Count == currentCombinationNumber)
                        {
                            var topPointer = pointers.Pop();
                            var topPol = productStack.Pop();

                            // Obtém o produto de todos os polinómios.
                            var currentPol = modularPolynomialDomain.Multiply(
                                topPol,
                                modularFactors[topPointer]);
                            currentPol = currentPol.ApplyFunction(
                            c => this.GetSymmetricRemainder(
                                c,
                                primePower,
                                halfPrimePower),
                            this.integerNumber);

                            // Compara as normas com o valor de teste
                            var firstNorm = this.GetPlynomialNorm(currentPol);
                            if (this.integerNumber.Compare(firstNorm, testValue) < 0)
                            {
                                if (modularProduct == null)
                                {
                                    modularProduct = this.ComputeModularProduct(
                                        modularFactors,
                                        modularPolynomialDomain);
                                }

                                var coPol = modularPolynomialDomain.Quo(
                                    modularProduct,
                                    currentPol);
                                coPol = coPol.ApplyFunction(
                                        c => this.GetSymmetricRemainder(
                                        c,
                                        primePower,
                                        halfPrimePower),
                                        this.integerNumber);
                                var secondNorm = this.GetPlynomialNorm(coPol);
                                var normProd = this.integerNumber.Multiply(firstNorm, secondNorm);
                                if (this.integerNumber.Compare(normProd, testValue) < 0)
                                {
                                    integerFactors.Add(currentPol);
                                    modularFactors.RemoveAt(topPointer);
                                    while (pointers.Count > 0)
                                    {
                                        var removeIndex = pointers.Pop();
                                        modularFactors.RemoveAt(removeIndex);
                                    }

                                    productStack.Clear();
                                    if (modularFactors.Count < currentCombinationNumber)
                                    {
                                        state = -1;
                                    }
                                    else
                                    {
                                        productStack.Push(modularFactors[0]);
                                        pointers.Push(0);
                                        pointers.Push(1);
                                    }
                                }
                                else
                                {
                                    productStack.Push(topPol);
                                    pointers.Push(topPointer);
                                    state = 1;
                                }
                            }
                            else
                            {
                                productStack.Push(topPol);
                                pointers.Push(topPointer);
                                state = 1;
                            }
                        }
                        else
                        {
                            var topPointer = pointers.Pop();
                            var topPol = productStack.Pop();
                            var currentPol = modularPolynomialDomain.Multiply(
                                topPol,
                                modularFactors[topPointer]);
                            currentPol = currentPol.ApplyFunction(
                                c => this.GetSymmetricRemainder(
                                            c,
                                            primePower,
                                            halfPrimePower),
                            this.integerNumber);

                            productStack.Push(topPol);
                            productStack.Push(currentPol);
                            pointers.Push(topPointer);
                            pointers.Push(topPointer + 1);
                        }
                    }
                    else if (state == 1)
                    {
                        var pointerLimit = modularFactors.Count - combinationsNumber + pointers.Count + 1;
                        if (pointers.Count > 1)
                        {
                            var topPointer = pointers.Pop();
                            ++topPointer;
                            if (topPointer < pointerLimit)
                            {
                                pointers.Push(topPointer);
                                state = 0;
                            }
                            else
                            {
                                productStack.Pop();
                            }
                        }
                        else
                        {
                            var topPointer = pointers.Pop();
                            ++topPointer;
                            if (topPointer < pointerLimit)
                            {
                                pointers.Push(topPointer);
                                productStack.Push(modularFactors[topPointer]);
                                pointers.Push(topPointer + 1);
                            }
                            else
                            {
                                ++currentCombinationNumber;
                                if (currentCombinationNumber < combinationsNumber)
                                {
                                    state = -1;
                                }
                                else if (modularFactors.Count < currentCombinationNumber)
                                {
                                    state = -1;
                                }
                                else
                                {
                                    pointers.Push(0);
                                    pointers.Push(1);
                                    productStack.Push(modularFactors[0]);
                                }
                            }
                        }
                    }
                }
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

        /// <summary>
        /// Determina o produto de todos os factores modulares.
        /// </summary>
        /// <param name="modularFactors">Os factores modulares.</param>
        /// <param name="modularDomain">O domínio modular.</param>
        /// <returns>O resultado do produto dos factores modulares.</returns>
        private UnivariatePolynomialNormalForm<CoeffType> ComputeModularProduct(
            List<UnivariatePolynomialNormalForm<CoeffType>> modularFactors,
            UnivarPolynomEuclideanDomain<CoeffType> modularDomain)
        {
            if (modularFactors.Count > 0)
            {
                var result = modularFactors[0];
                for (int i = 1; i < modularFactors.Count; ++i)
                {
                    result = modularDomain.Multiply(result, modularFactors[i]);
                }

                return result;
            }
            else
            {
                return modularDomain.MultiplicativeUnity;
            }
        }
    }
}
