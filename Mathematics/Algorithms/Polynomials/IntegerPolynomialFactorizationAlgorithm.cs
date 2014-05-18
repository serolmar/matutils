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
        /// O número de passos a efectuar no passo da pesquisa.
        /// </summary>
        private const int searchStepIterations = 3;

        /// <summary>
        /// O objecto responsável pelas operações sobre os números inteiros.
        /// </summary>
        private IIntegerNumber<CoeffType> integerNumber;

        /// <summary>
        /// O corpo responsável pelas operações sobre as fracções.
        /// </summary>
        private FractionField<CoeffType> fractionField;

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

        /// <summary>
        /// O algoritmo que permite determinar o resultante entre dois polinómios.
        /// </summary>
        /// <remarks>
        /// Trata-se de um algoritmo útil na obtenção do discriminante de um polinómio, o qual
        /// é definido como o resultante entre esse polinómio e a sua derivada.
        /// </remarks>
        private IAlgorithm<UnivariatePolynomialNormalForm<CoeffType>,
            UnivariatePolynomialNormalForm<CoeffType>, CoeffType> resultantAlg;

        /// <summary>
        /// O algoritmo que implementa a raiz quadrada inteira de um número.
        /// </summary>
        private IAlgorithm<int, int> integerSquareRootAlgorithm;

        /// <summary>
        /// Uma fábrica responsável pela construção de um objecto responsável pela iteração
        /// do conjunto de números primos limitado por um determinado valor.
        /// </summary>
        private IPrimeNumberIteratorFactory<CoeffType> primeNumbersIteratorFactory;

        /// <summary>
        /// O algoritmo que permite calcular o valor do logaritmo de um número.
        /// </summary>
        private IAlgorithm<CoeffType, double> logarithmAlgorithm;

        /// <summary>
        /// O algoritmo responsável pelo levantamento da factorização em corpos finitos.
        /// </summary>
        IAlgorithm<MultiFactorLiftingStatus<CoeffType>,int, MultiFactorLiftingResult<CoeffType>> 
            multiFactorLiftingAlg;

        /// <summary>
        /// Constrói uma instância de um objecto responsável pela factorização de um polinómio
        /// cujos coeficientes são fracções inteiras.
        /// </summary>
        /// <param name="integerNumber">O objecto responsável pelas operações sobre os números inteiros.</param>
        /// <param name="modularSymmetricFactory">
        /// A fábrica responsável pela ciração de instâncias de objectos
        /// responsáveis pelas operações modulares.
        /// </param>
        /// <param name="primeNumbersIteratorFactory">
        /// A fábrica responsável pela criação de um objecto capaz de iterar sobre o conjunto de números
        /// primos limitados por um determinado valor.
        /// </param>
        /// <param name="logarithmAlgorithm">O objecto responsável pelo cálculo do logaritmo.</param>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        public IntegerPolynomialFactorizationAlgorithm(
            IIntegerNumber<CoeffType> integerNumber,
            IModularFieldFactory<CoeffType> modularSymmetricFactory,
            IPrimeNumberIteratorFactory<CoeffType> primeNumbersIteratorFactory,
            IAlgorithm<CoeffType, double> logarithmAlgorithm)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (modularSymmetricFactory == null)
            {
                throw new ArgumentNullException("modularSymmetricFactory");
            }
            else if (primeNumbersIteratorFactory == null)
            {
                throw new ArgumentNullException("primeNumbersIteratorFactory");
            }
            else if (logarithmAlgorithm == null)
            {
                throw new ArgumentNullException("logarithmAlgorithm");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.fractionField = new FractionField<CoeffType>(integerNumber);
                this.modularSymmetricFactory = modularSymmetricFactory;
                this.squareFreeAlg = new SquareFreeFractionFactorizationAlg<CoeffType>(integerNumber);
                this.searchFactorizationAlgorithm = new SearchFactorizationAlgorithm<CoeffType>(
                    modularSymmetricFactory,
                    this.integerNumber);
                this.integerSquareRootAlgorithm = new IntegerSquareRootAlgorithm();
                this.primeNumbersIteratorFactory = primeNumbersIteratorFactory;
                this.logarithmAlgorithm = logarithmAlgorithm;
                this.resultantAlg = new UnivarPolDeterminantResultantAlg<CoeffType>(
                    this.integerNumber);

                // O algoritmo responsável pelo levantamento módulo uma potência de um número primo.
                this.multiFactorLiftingAlg = new MultiFactorLiftAlgorithm<CoeffType>(
                    new LinearLiftAlgorithm<CoeffType>(
                        modularSymmetricFactory,
                        new UnivarPolEuclideanDomainFactory<CoeffType>(),
                        integerNumber));
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
                        var currentPolynomial = squareFreeFactorKvp.Value;
                        var polynomialLeadingCoeff = currentPolynomial.GetLeadingCoefficient(this.integerNumber);
                        var polynomialNorm = this.GetPolynomialNorm(currentPolynomial);
                        var bound = this.ComputeMignotteApproximation(
                            polynomialNorm,
                            polynomialLeadingCoeff,
                            polynomial.Degree);

                        var primeNumbersBound = this.ComputeGamma(polynomialNorm, polynomial.Degree);
                        var primeNumbersIterator = this.primeNumbersIteratorFactory.CreatePrimeNumberIterator(
                            primeNumbersBound);
                        var primeNumbersEnumerator = primeNumbersIterator.GetEnumerator();
                        var discriminant = this.resultantAlg.Run(
                            currentPolynomial,
                            currentPolynomial.GetPolynomialDerivative(this.integerNumber));
                        var currentPrime = this.integerNumber.MultiplicativeUnity;
                        var status = 0;
                        while (status == 0)
                        {
                            if (primeNumbersEnumerator.MoveNext())
                            {
                                currentPrime = primeNumbersEnumerator.Current;
                                if (!this.integerNumber.IsAdditiveUnity(
                                    this.integerNumber.Rem(polynomialLeadingCoeff, currentPrime)) &&
                                    !this.integerNumber.IsAdditiveUnity(this.integerNumber.Rem(
                                    discriminant,
                                    currentPrime)))
                                {
                                    status = 1;
                                }
                            }
                            else
                            {
                                status = -1;
                            }
                        }

                        if (status == 1) // Encontrou um número primo sobre o qual é possível proceder à elevação.
                        {
                            var factorList = new List<UnivariatePolynomialNormalForm<CoeffType>>();
                            var iterationsNumber = this.GetHenselLiftingIterationsNumber(bound, currentPrime);
                            var coeff = this.FactorizePolynomial(
                                currentPolynomial,
                                currentPrime,
                                bound,
                                iterationsNumber,
                                factorList);

                            var multiplicationCoeff = this.fractionField.InverseConversion(coeff);
                            multiplicationCoeff = MathFunctions.Power(
                                multiplicationCoeff,
                                squareFreeFactorKvp.Key,
                                this.fractionField);

                            independentCoeff = this.fractionField.Multiply(independentCoeff, multiplicationCoeff);
                            factorizationDictionary.Add(
                            squareFreeFactorKvp.Key,
                            factorList);
                        }
                        else
                        {
                            factorizationDictionary.Add(
                            squareFreeFactorKvp.Key,
                            new List<UnivariatePolynomialNormalForm<CoeffType>>() { squareFreeFactorKvp.Value });
                        }
                    }
                }

                return new PolynomialFactorizationResult<Fraction<CoeffType>, CoeffType>(
                    independentCoeff,
                    factorizationDictionary);
            }
        }

        /// <summary>
        /// Factoriza o polinómio aplicando os métodos conhecidos.
        /// </summary>
        /// <param name="polynomial">O polinómio a ser factorizado.</param>
        /// <param name="primeNumber">O número primo.</param>
        /// <param name="bound">O limite dos coeficientes que podem representar números inteiros.</param>
        /// <param name="iterationsNumber">O número de iterações para o algoritmo do levantamento multifactor.</param>
        /// <param name="factorsList">A lista de factores.</param>
        /// <returns>O coeficiente independente.</returns>
        private CoeffType FactorizePolynomial(
            UnivariatePolynomialNormalForm<CoeffType> polynomial,
            CoeffType primeNumber,
            CoeffType bound,
            int iterationsNumber,
            List<UnivariatePolynomialNormalForm<CoeffType>> factorsList)
        {
            var modularField = this.modularSymmetricFactory.CreateInstance(primeNumber);
            var linearSystemSolver = new DenseCondensationLinSysAlgorithm<CoeffType>(
                modularField);
            var finiteFieldFactAlg = new FiniteFieldPolFactorizationAlgorithm<CoeffType>(
                linearSystemSolver,
                this.integerNumber);
            var finiteFieldFactorizationResult = finiteFieldFactAlg.Run(
                polynomial,
                modularField);
            var multiFactorLiftingStatus = new MultiFactorLiftingStatus<CoeffType>(
                polynomial,
                finiteFieldFactorizationResult,
                primeNumber);
            var multiFactorLiftingResult = this.multiFactorLiftingAlg.Run(
                multiFactorLiftingStatus,
                iterationsNumber);
            var searchResult = this.searchFactorizationAlgorithm.Run(
                multiFactorLiftingResult,
                bound,
                3);

            if (searchResult.IntegerFactors.Count > 0)
            {
                factorsList.AddRange(searchResult.IntegerFactors);
                if (searchResult.NonIntegerFactors.Count > 0) // É necessário factorizar.
                {
                    var currentFactor = searchResult.IntegerFactors[0];
                    for (int i = 1; i < searchResult.IntegerFactors.Count; ++i)
                    {
                        currentFactor = currentFactor.Multiply(
                            searchResult.IntegerFactors[i],
                            this.integerNumber);
                    }

                    var nonFactored = MathFunctions.GetIntegerDivision(
                        searchResult.MainPolynomial,
                        currentFactor,
                        this.integerNumber);
                }
            }
            else
            {
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Permite calcular a aproximação de Mignotte relativamente à factorização de polinómios.
        /// </summary>
        /// <remarks>
        /// A aproximaçáo calculada consiste num limite superior para os coeficientes de um polinómio
        /// módulo a potência de um número primo caso este seja factor do polinómio proposto.
        /// </remarks>
        /// <param name="polynomialNorm">A norma do polinómio.</param>
        /// <param name="leadingCoeff">O coeficiente principal do polinómio.</param>
        /// <param name="degree">O grau do polinómio.</param>
        /// <returns>O valor da aproximação.</returns>
        private CoeffType ComputeMignotteApproximation(
            CoeffType polynomialNorm,
            CoeffType leadingCoeff,
            int degree)
        {
            var result = MathFunctions.Power(this.integerNumber.MapFrom(2), degree, this.integerNumber);
            result = this.integerNumber.Multiply(result, leadingCoeff);
            result = this.integerNumber.Multiply(result, polynomialNorm);

            // Utilização da expansão em série para a determinação do valor inteiro da raiz quadrada.
            // a_1=1, b_1=2, a_{n+1}=-(2n+1)a_n, b_{n+1}=2(n+1)b_n
            var integerSquareRoot = this.integerNumber.MapFrom(this.integerSquareRootAlgorithm.Run(degree + 1));
            var difference = this.integerNumber.Multiply(integerSquareRoot, integerSquareRoot);
            var squared = this.integerNumber.Multiply(integerSquareRoot, integerSquareRoot);
            difference = this.integerNumber.Add(
                this.integerNumber.MapFrom(degree + 1),
                this.integerNumber.AdditiveInverse(squared));
            if (this.integerNumber.IsAdditiveUnity(difference))
            {
                result = this.integerNumber.Multiply(result, integerSquareRoot);
            }
            else
            {
                var db = this.integerNumber.MapFrom(2);
                var numMult = this.integerNumber.MultiplicativeUnity;
                var denMult = this.integerNumber.MultiplicativeUnity;
                var sign = true;
                var numeratorCoeff = this.integerNumber.Multiply(result, difference);
                var denominatorCoeff = db;
                denominatorCoeff = this.integerNumber.Multiply(denominatorCoeff, integerSquareRoot);
                var gcd = MathFunctions.GreatCommonDivisor(numeratorCoeff, denominatorCoeff, this.integerNumber);
                if (!this.integerNumber.IsMultiplicativeUnity(gcd))
                {
                    numeratorCoeff = this.integerNumber.Quo(numeratorCoeff, gcd);
                    denominatorCoeff = this.integerNumber.Quo(denominatorCoeff, gcd);
                }

                result = this.integerNumber.Multiply(result, integerSquareRoot);
                if (this.integerNumber.Compare(numeratorCoeff, denominatorCoeff) > 0)
                {
                    // Adicionar a primeira parcela da expansão em série.
                    var divide = this.integerNumber.Quo(numeratorCoeff, denominatorCoeff);
                    if (!sign)
                    {
                        divide = this.integerNumber.AdditiveInverse(divide);
                    }

                    result = this.integerNumber.Add(result, divide);

                    // Actualiza com a nova informação.
                    sign = !sign;
                    denMult = this.integerNumber.Successor(denMult);
                    numeratorCoeff = this.integerNumber.Multiply(numeratorCoeff, difference);
                    numeratorCoeff = this.integerNumber.Multiply(numeratorCoeff, numMult);
                    denominatorCoeff = this.integerNumber.Multiply(denominatorCoeff, denMult);
                    denominatorCoeff = this.integerNumber.Multiply(denominatorCoeff, squared);
                    denominatorCoeff = this.integerNumber.Multiply(denominatorCoeff, db);
                    gcd = MathFunctions.GreatCommonDivisor(numeratorCoeff, denominatorCoeff, this.integerNumber);
                    if (!this.integerNumber.IsMultiplicativeUnity(gcd))
                    {
                        numeratorCoeff = this.integerNumber.Quo(numeratorCoeff, gcd);
                        denominatorCoeff = this.integerNumber.Quo(denominatorCoeff, gcd);
                    }

                    while (this.integerNumber.Compare(numeratorCoeff, denominatorCoeff) > 0)
                    {
                        // Adicionar a primeira parcela da expansão em série.
                        divide = this.integerNumber.Quo(numeratorCoeff, denominatorCoeff);
                        if (!sign)
                        {
                            divide = this.integerNumber.AdditiveInverse(divide);
                        }

                        result = this.integerNumber.Add(result, divide);

                        // Actualiza com a nova informação.
                        sign = !sign;
                        numMult = this.integerNumber.Successor(this.integerNumber.Successor(numMult));
                        denMult = this.integerNumber.Successor(denMult);
                        numeratorCoeff = this.integerNumber.Multiply(numeratorCoeff, difference);
                        numeratorCoeff = this.integerNumber.Multiply(numeratorCoeff, numMult);
                        denominatorCoeff = this.integerNumber.Multiply(denominatorCoeff, denMult);
                        denominatorCoeff = this.integerNumber.Multiply(denominatorCoeff, squared);
                        denominatorCoeff = this.integerNumber.Multiply(denominatorCoeff, db);
                        this.integerNumber.Multiply(denominatorCoeff, db);
                        gcd = MathFunctions.GreatCommonDivisor(numeratorCoeff, denominatorCoeff, this.integerNumber);
                        if (!this.integerNumber.IsMultiplicativeUnity(gcd))
                        {
                            numeratorCoeff = this.integerNumber.Quo(numeratorCoeff, gcd);
                            denominatorCoeff = this.integerNumber.Quo(denominatorCoeff, gcd);
                        }
                    }
                }

                if (sign)
                {
                    result = this.integerNumber.Successor(result);
                }
            }

            return result;
        }

        /// <summary>
        /// Permite obter a norma de um polinómio.
        /// </summary>
        /// <param name="polynomial">O polinómio do qual se pretende obter a norma.</param>
        /// <returns>O valor da norma.</returns>
        private CoeffType GetPolynomialNorm(UnivariatePolynomialNormalForm<CoeffType> polynomial)
        {
            var result = this.integerNumber.AdditiveUnity;
            var polynomialEnumerator = polynomial.GetEnumerator();
            if (polynomialEnumerator.MoveNext())
            {
                result = polynomialEnumerator.Current.Value;
                while (polynomialEnumerator.MoveNext())
                {
                    var current = polynomialEnumerator.Current.Value;
                    if (this.integerNumber.Compare(current, result) > 0)
                    {
                        result = current;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém o valor da estimativa para o valor máximo dos números primos a serem gerados.
        /// </summary>
        /// <param name="polynomialNorm">A norma do polinómio.</param>
        /// <param name="degree">O grau do polinómio.</param>
        /// <returns>O valor para o número primo máximo.</returns>
        private CoeffType ComputeGamma(CoeffType polynomialNorm, int degree)
        {
            var result = this.integerNumber.Successor(
                this.integerNumber.MapFrom(degree));
            var doubleDegree = 2 * degree;
            result = MathFunctions.Power(result, doubleDegree, this.integerNumber);
            var temp = MathFunctions.Power(polynomialNorm, doubleDegree + 1, this.integerNumber);
            result = this.integerNumber.Multiply(result, temp);
            var doubleLogResult = this.logarithmAlgorithm.Run(result) * 2;
            doubleLogResult = Math.Ceiling(doubleLogResult);
            doubleLogResult = 2 * doubleLogResult * Math.Log(doubleLogResult);
            return this.integerNumber.MapFrom((int)Math.Floor(doubleLogResult));
        }

        /// <summary>
        /// Permite obter o número de iterações relativo à aplicação da elevação de Hensel
        /// à factorização em corpos finitos.
        /// </summary>
        /// <param name="approximation">O limite de Mignotte.</param>
        /// <param name="primeNumber">O número primo.</param>
        /// <returns>O número de iterações.</returns>
        private int GetHenselLiftingIterationsNumber(
            CoeffType approximation,
            CoeffType primeNumber)
        {
            var logArg = this.integerNumber.Multiply(
                this.integerNumber.MapFrom(2),
                approximation);
            logArg = this.integerNumber.Successor(logArg);
            var logResult = this.logarithmAlgorithm.Run(logArg);
            logResult = logResult / this.logarithmAlgorithm.Run(primeNumber);
            return (int)Math.Ceiling(logResult);
        }

        /// <summary>
        /// Determina o corte de um número sobre as potências de um número primo.
        /// </summary>
        /// <param name="value">O valor do qual se pretende obter o corte.</param>
        /// <param name="prime">O número primo.</param>
        /// <param name="firstExponent">O expoente inferior do corte.</param>
        /// <param name="secondExponent">O expoente superior do corte.</param>
        /// <returns>O resultado do corte.</returns>
        private CoeffType ComputeTwoSidedCut(
            CoeffType value, 
            CoeffType prime,
            CoeffType firstExponent, 
            CoeffType secondExponent)
        {
            var difference = this.integerNumber.Add(
                secondExponent,
                this.integerNumber.AdditiveInverse(firstExponent));
            if (this.integerNumber.Compare(firstExponent, difference) < 0)
            {
                // Cálculo da primeira potência.
                var firstExponentFactor = MathFunctions.Power(
                    prime, 
                    firstExponent, 
                    this.integerNumber, 
                    this.integerNumber);

                // Cálculo da segunda potência com base na primeira.
                var increment = this.integerNumber.Add(
                    firstExponent,
                    this.integerNumber.AdditiveInverse(difference));
                var differenceExponentFactor = MathFunctions.Power(
                    prime,
                    increment,
                    this.integerNumber,
                    this.integerNumber);
                differenceExponentFactor = this.integerNumber.Multiply(
                    differenceExponentFactor,
                    firstExponentFactor);

                var result = this.integerNumber.Rem(value, firstExponentFactor);
                result = this.integerNumber.Add(
                    value,
                    this.integerNumber.AdditiveInverse(result));
                result = this.integerNumber.Quo(result, firstExponentFactor);
                result = this.integerNumber.Rem(result, differenceExponentFactor);
                return result;
            }
            else
            {
                // Cálculo da primeira potência.
                var differenceExponentFactor = MathFunctions.Power(
                    prime, 
                    difference,
                    this.integerNumber,
                    this.integerNumber);

                // Cálculo da segunda potência com base na primeira.
                var increment = this.integerNumber.Add(
                    difference,
                    this.integerNumber.AdditiveInverse(firstExponent));
                var firstExponentFactor = MathFunctions.Power(
                    prime,
                    increment,
                    this.integerNumber,
                    this.integerNumber);
                firstExponentFactor = this.integerNumber.Multiply(
                    firstExponentFactor,
                    firstExponentFactor);

                var result = this.integerNumber.Rem(value, firstExponentFactor);
                result = this.integerNumber.Add(
                    value,
                    this.integerNumber.AdditiveInverse(result));
                result = this.integerNumber.Quo(result, firstExponentFactor);
                result = this.integerNumber.Rem(result, differenceExponentFactor);
                return result;
            }
        }
    }
}
