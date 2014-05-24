namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implemneta o crivo sobre corpos quadráticos para determinar uma factorização para
    /// qualquer número.
    /// </summary>
    public class QuadraticFieldSieve<NumberType> : IAlgorithm<NumberType, int, int, Tuple<NumberType, NumberType>>
    {
        /// <summary>
        /// Algoritmo que permite resolver o sistema de equações.
        /// </summary>
        private IAlgorithm<IMatrix<int>, IMatrix<int>, LinearSystemSolution<int>>
            linearSystemAlgorithm;

        /// <summary>
        /// Permite criar instâncias do corpo modular.
        /// </summary>
        private IModularFieldFactory<NumberType> modularFieldFactory;

        /// <summary>
        /// Permite criar instâncias de iteradores para números primos positivos.
        /// </summary>
        IPrimeNumberIteratorFactory<int> primesIteratorFactory;

        /// <summary>
        /// Permite determinar a parte inteira da raiz quadrada.
        /// </summary>
        IAlgorithm<NumberType, NumberType> integerSquareRootAlgorithm;

        /// <summary>
        /// Permite efectuar as operações sobre os números inteiros.
        /// </summary>
        IIntegerNumber<NumberType> integerNumber;

        /// <summary>
        /// O corpo responsável pelas operações módulo dois.
        /// </summary>
        private ModularIntegerField field;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="QuadraticFieldSieve{NumberType}"/>.
        /// </summary>
        /// <param name="integerSquareRootAlgorithm">
        /// O objecto responsável pelo cálculo da parte inteira da raiz quadrada de um número.
        /// </param>
        /// <param name="modularFieldFactory">
        /// A fábrica responsável pela criação de objectos capazes de efectuar operações modulares.
        /// </param>
        /// <param name="primesIteratorFactory">
        /// A fábrica responsável pela criação de enumeradores para números primos.
        /// </param>
        /// <param name="integerNumber">O objecto responsável pelas operações sobre os números inteiros.</param>
        /// <exception cref="ArgumentNullException">
        /// Se pelo menos um dos argumentos for nulo.
        /// </exception>
        public QuadraticFieldSieve(
            IAlgorithm<NumberType, NumberType> integerSquareRootAlgorithm,
            IModularFieldFactory<NumberType> modularFieldFactory,
            IPrimeNumberIteratorFactory<int> primesIteratorFactory,
            IIntegerNumber<NumberType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (primesIteratorFactory == null)
            {
                throw new ArgumentNullException("primesIteratorFactory");
            }
            else if (modularFieldFactory == null)
            {
                throw new ArgumentNullException("modularFieldFactory");
            }
            else if (integerSquareRootAlgorithm == null)
            {
                throw new ArgumentNullException("integerSquareRootAlgorithm");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.modularFieldFactory = modularFieldFactory;
                this.field = new ModularIntegerField(2);
                this.linearSystemAlgorithm = new DenseCondensationLinSysAlgorithm<int>(
                    this.field);
                this.primesIteratorFactory = primesIteratorFactory;
                this.integerSquareRootAlgorithm = integerSquareRootAlgorithm;
            }
        }

        /// <summary>
        /// Obtém a factorização do módulo do número especificado.
        /// </summary>
        /// <param name="data">O número.</param>
        /// <param name="factorBase">O limite máximo para os números primos da base.</param>
        /// <param name="sieveInterval">O intervalo sobre os quais são crivados os números.</param>
        /// <returns>A decomposição do número especificado num produto de dois factores.</returns>
        /// <exception cref="ArgumentException">
        /// Se o limite for inferior a dois ou o intervalo for inferior a um.
        /// </exception>
        /// <exception cref="MathematicsException">Se o número for zero.</exception>
        public Tuple<NumberType, NumberType> Run(NumberType data, int factorBase, int sieveInterval)
        {
            var innerData = this.integerNumber.GetNorm(data);
            var two = this.integerNumber.MapFrom(2);
            if (factorBase < 2)
            {
                throw new ArgumentException("Factor base limit can't be less than two.");
            }
            else if (sieveInterval < 1)
            {
                throw new ArgumentException("Sieve interval can't be less than one.");
            }
            if (this.integerNumber.IsAdditiveUnity(innerData))
            {
                throw new MathematicsException("Zero has no factors.");
            }
            else if (this.integerNumber.IsMultiplicativeUnity(innerData) ||
                this.integerNumber.Equals(innerData, two) ||
                this.integerNumber.Equals(innerData, this.integerNumber.MapFrom(3)))
            {
                return Tuple.Create(this.integerNumber.MultiplicativeUnity, innerData);
            }
            else
            {
                var primesList = new List<int>();
                var primesIterator = this.primesIteratorFactory.CreatePrimeNumberIterator(factorBase);
                var legendreAlgorithm = new LegendreJacobiSymbolAlgorithm<NumberType>(this.integerNumber);
                foreach (var prime in primesIterator)
                {
                    if (this.integerNumber.IsMultiplicativeUnity(legendreAlgorithm.Run(innerData, this.integerNumber.MapFrom(prime))))
                    {
                        primesList.Add(prime);
                    }
                }

                if (primesList.Count == 0 || primesList[0] != 2)
                {
                    primesList.Insert(0, 2);
                }

                var sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                var prod = this.integerNumber.Multiply(sqrt, sqrt);
                if (this.integerNumber.Equals(prod, innerData))
                {
                    // Neste caso já encontrámos um factor
                    return Tuple.Create(sqrt, sqrt);
                }
                else
                {
                    var sieveMatrix = this.ComputeSieveStep(innerData, sqrt, sieveInterval, primesList);

                    var modularMatrix = this.GetBitMatrixFromList(sieveMatrix, primesList);

                    // Implementação do algoritmo associado à combinação linear para a obtenção do resultado
                    var solution = this.linearSystemAlgorithm.Run(
                        modularMatrix,
                        new ZeroMatrix<int>(
                            modularMatrix.GetLength(0),
                            1,
                            this.field));

                    return this.GetSolution(solution, sieveMatrix, primesList, innerData);
                }
            }
        }

        /// <summary>
        /// Obtém os valores da solução.
        /// </summary>
        /// <param name="solution">A solução do sistema modular.</param>
        /// <param name="matrixList">A matriz.</param>
        /// <param name="primesList">A lista dos números primos da base.</param>
        /// <param name="innerData">O número que está a ser factorizado.</param>
        /// <returns>Os factores.</returns>
        private Tuple<NumberType, NumberType> GetSolution(
            LinearSystemSolution<int> solution,
            List<int[]> matrixList,
            List<int> primesList,
            NumberType innerData)
        {
            var innerDataModularField = this.modularFieldFactory.CreateInstance(innerData);
            var countFactors = primesList.Count - 1;
            foreach (var solutionBase in solution.VectorSpaceBasis)
            {
                var firstValue = this.integerNumber.MultiplicativeUnity;
                var factorsCount = new Dictionary<int, int>();
                for (int i = 0; i < matrixList.Count; ++i)
                {
                    var currentMatrixLine = matrixList[i];
                    if (solutionBase[i] == 1)
                    {
                        firstValue = innerDataModularField.Multiply(
                            firstValue,
                            this.integerNumber.GetNorm(this.integerNumber.MapFrom(currentMatrixLine[currentMatrixLine.Length - 1])));

                        for (int j = 0; j < countFactors; ++j)
                        {
                            if (currentMatrixLine[j] != 0)
                            {
                                var countValue = 0;
                                if (factorsCount.TryGetValue(primesList[j], out countValue))
                                {
                                    countValue += currentMatrixLine[j];
                                    factorsCount[primesList[j]] = countValue;
                                }
                                else
                                {
                                    factorsCount.Add(primesList[j], currentMatrixLine[j]);
                                }
                            }
                        }
                    }
                }

                var secondValue = this.integerNumber.MultiplicativeUnity;
                foreach (var factorCountKvp in factorsCount)
                {
                    var primePower = MathFunctions.Power(
                        this.integerNumber.MapFrom(factorCountKvp.Key),
                        factorCountKvp.Value / 2,
                        innerDataModularField);

                    secondValue = innerDataModularField.Multiply(
                        secondValue,
                        primePower);
                }

                if (!this.integerNumber.Equals(firstValue, secondValue))
                {
                    var firstFactor = MathFunctions.GreatCommonDivisor(
                        innerData,
                        this.integerNumber.GetNorm(this.integerNumber.Add(firstValue, this.integerNumber.AdditiveInverse(secondValue))),
                        this.integerNumber);
                    if (!this.integerNumber.IsMultiplicativeUnity(firstFactor))
                    {
                        var secondFactor = this.integerNumber.Quo(innerData, firstFactor);
                        return Tuple.Create(firstFactor, secondFactor);
                    }
                }
            }

            return Tuple.Create(this.integerNumber.MultiplicativeUnity, innerData);
        }

        /// <summary>
        /// Determina a matriz que resulta da aplicação do crivo quadrático.
        /// </summary>
        /// <param name="innerData">O valor a ser factorizado.</param>
        /// <param name="sqrt">O valor da raiz quadrada.</param>
        /// <param name="sieveInterval">O intervalo de crivo.</param>
        /// <param name="primesList">A base de primos.</param>
        /// <returns>A matriz com o vector.</returns>
        private List<int[]> ComputeSieveStep(
            NumberType innerData,
            NumberType sqrt,
            int sieveInterval,
            List<int> primesList)
        {
            var innerSieveInterval = this.integerNumber.MapFrom(sieveInterval);
            var tempInterval = this.integerNumber.Add(
                innerData,
                this.integerNumber.AdditiveInverse(this.integerNumber.Successor(sqrt)));
            if (this.integerNumber.Compare(innerSieveInterval, tempInterval) > 0)
            {
                innerSieveInterval = tempInterval;
            }

            var sieveValues = this.ComputeSieveValues(innerData, sieveInterval, sqrt);
            var sieve = new int[sieveValues.Length];
            Array.Copy(sieveValues, sieve, sieveValues.Length);
            var matrixList = new List<int[]>();
            for (int i = 0; i < sieveValues.Length; ++i)
            {
                // Inclui o sinal
                matrixList.Add(new int[primesList.Count + 2]);
            }

            var resSolAlg = new ResSolAlgorithm<NumberType>(
                this.modularFieldFactory,
                this.integerNumber);
            for (int i = 1; i < primesList.Count; ++i)
            {
                var currentPrime = this.integerNumber.MapFrom(primesList[i]);
                var quadraticRes = resSolAlg.Run(innerData, currentPrime);
                var symmSqrt = this.integerNumber.AdditiveInverse(sqrt);
                var firstRes = this.integerNumber.Rem(this.integerNumber.Add(quadraticRes[0], symmSqrt), currentPrime);
                var secondRes = this.integerNumber.Rem(this.integerNumber.Add(quadraticRes[1], symmSqrt), currentPrime);
                var tempResult = this.integerNumber.AdditiveInverse(secondRes);
                tempResult = this.integerNumber.Add(firstRes, tempResult);
                tempResult = this.integerNumber.Rem(tempResult, currentPrime);
                if (this.integerNumber.IsAdditiveUnity(tempResult))
                {
                    // Apenas uma solução encontrada
                    this.SetupQuadResSolution(
                        this.integerNumber.ConvertToInt(firstRes),
                        primesList[i],
                        i,
                        sieveInterval,
                        sieveValues,
                        matrixList);
                }
                else
                {
                    // Duas soluções encontradas
                    this.SetupQuadResSolution(
                        this.integerNumber.ConvertToInt(firstRes),
                        primesList[i],
                        i,
                        sieveInterval,
                        sieveValues,
                        matrixList);

                    this.SetupQuadResSolution(
                        this.integerNumber.ConvertToInt(secondRes),
                        primesList[i],
                        i,
                        sieveInterval,
                        sieveValues,
                        matrixList);
                }
            }

            // Tratamento do número primo 2
            var two = this.integerNumber.MapFrom(2);
            var eight = this.integerNumber.MapFrom(8);
            if (this.integerNumber.IsMultiplicativeUnity(this.integerNumber.Rem(innerData, eight)))
            {
                var index = 0;
                while (index < sieveValues.Length &&
                    !this.integerNumber.IsAdditiveUnity(this.integerNumber.Rem(sieveValues[index], two)))
                {
                    ++index;
                }

                while (index < sieveValues.Length)
                {
                    var factorValue = sieveValues[index];
                    var factorPower = 0;
                    var remQuoResult = this.integerNumber.GetQuotientAndRemainder(factorValue, two);
                    while (this.integerNumber.IsAdditiveUnity(remQuoResult.Remainder))
                    {
                        ++factorPower;
                        factorValue = remQuoResult.Quotient;
                        remQuoResult = this.integerNumber.GetQuotientAndRemainder(factorValue, two);
                    }

                    sieveValues[index] = factorValue;
                    matrixList[index][0] = factorPower;
                    index += 2;
                }
            }
            else
            {
                var index = 0;
                while (index < sieveValues.Length &&
                    !this.integerNumber.IsAdditiveUnity(this.integerNumber.Rem(sieveValues[index], two)))
                {
                    ++index;
                }

                while (index < sieveValues.Length)
                {
                    var factorValue = this.integerNumber.Quo(sieveValues[index], two);
                    sieveValues[index] = factorValue;
                    matrixList[index][0] = 1;
                    index += 2;
                }
            }

            // Tratamento do sinal
            primesList.Add(-1);
            for (int i = 0; i < matrixList.Count; ++i)
            {
                if (this.integerNumber.Compare(sieveValues[i], this.integerNumber.AdditiveUnity) < 0)
                {
                    var currentLine = matrixList[i];
                    currentLine[currentLine.Length - 2] = 1;
                }
            }

            // Obtém a matriz dos vectores respeitantes aos números suaves
            var matrixIndex = 0;
            for (int i = 0; i < sieveValues.Length; ++i)
            {
                var currentSieveValue = sieveValues[i];
                if (this.integerNumber.IsMultiplicativeUnity(currentSieveValue) ||
                    this.integerNumber.IsMultiplicativeUnity(this.integerNumber.AdditiveInverse(currentSieveValue)))
                {
                    var currentLine = matrixList[matrixIndex];
                    currentLine[currentLine.Length - 1] = sieve[i];
                    ++matrixIndex;
                }
                else
                {
                    matrixList.RemoveAt(matrixIndex);
                }
            }

            return matrixList;
        }

        /// <summary>
        /// Obtém a matriz sobre a qual se pretende obter o espaço nulo.
        /// </summary>
        /// <param name="matrixList">A matriz original.</param>
        /// <param name="primesList">A lista dos primos.</param>
        /// <returns>A matriz de "bits".</returns>
        private ArrayBitMatrix GetBitMatrixFromList(List<int[]> matrixList, List<int> primesList)
        {
            var matrix = new ArrayBitMatrix(primesList.Count, matrixList.Count, 0);
            for (int i = 0; i < primesList.Count; ++i)
            {
                for (int j = 0; j < matrixList.Count; ++j)
                {
                    matrix[i, j] = matrixList[j][i] % 2;
                }
            }

            return matrix;
        }

        /// <summary>
        /// Calcula todos os valores respeitantes ao intervalo do crivo.
        /// </summary>
        /// <param name="innerData">O número a ser factorizado.</param>
        /// <param name="sieveInterval">O intervalo do crivo.</param>
        /// <param name="sqrt">A raíz quadrada.</param>
        /// <returns>Os valores do crivo.</returns>
        private NumberType[] ComputeSieveValues(NumberType innerData, int sieveInterval, NumberType sqrt)
        {
            var length = 2 * sieveInterval + 1;
            var result = new NumberType[2 * sieveInterval + 1];
            result[sieveInterval] = sqrt;
            for (int i = 1; i <= sieveInterval; ++i)
            {
                var plus = this.integerNumber.MapFrom(i);
                var minus = this.integerNumber.MapFrom(-i);
                var addition = this.integerNumber.Add(sqrt, plus);
                var difference = this.integerNumber.Add(sqrt, minus);
                result[sieveInterval - i] = this.integerNumber.Add(this.integerNumber.Multiply(difference, difference), this.integerNumber.AdditiveInverse(innerData));
                result[sieveInterval + i] = this.integerNumber.Add(this.integerNumber.Multiply(addition, addition), this.integerNumber.AdditiveInverse(innerData));
            }

            return result;
        }

        /// <summary>
        /// Processa os valores obtidos com o auxílio da solução da congruência quadrática.
        /// </summary>
        /// <param name="solution">A solução.</param>
        /// <param name="currentPrime">O número primo em tratamento.</param>
        /// <param name="currentPrimeIndex">A posição do número primo na base.</param>
        /// <param name="sieveInterval">O intervalo do crivo.</param>
        /// <param name="sieve">O crivo quadrático.</param>
        /// <param name="matrix">A matriz dos expoentes.</param>
        private void SetupQuadResSolution(
            int solution,
            int currentPrime,
            int currentPrimeIndex,
            int sieveInterval,
            NumberType[] sieve,
            List<int[]> matrix)
        {
            var innerSolution = solution - currentPrime;
            var mappedPrime = this.integerNumber.MapFrom(currentPrime);
            if (solution < sieveInterval)
            {
                while (innerSolution > -sieveInterval)
                {
                    var index = innerSolution + sieveInterval;
                    var factorValue = sieve[index];
                    var factorPower = 0;
                    var remQuoRes = this.integerNumber.GetQuotientAndRemainder(factorValue, mappedPrime);
                    while (this.integerNumber.IsAdditiveUnity(remQuoRes.Remainder))
                    {
                        ++factorPower;
                        factorValue = remQuoRes.Quotient;
                        remQuoRes = this.integerNumber.GetQuotientAndRemainder(factorValue, mappedPrime);
                    }

                    sieve[index] = factorValue;
                    matrix[index][currentPrimeIndex] = factorPower;

                    innerSolution -= currentPrime;
                }
            }

            innerSolution = solution;
            if (solution > -sieveInterval)
            {
                while (innerSolution < sieveInterval)
                {
                    var index = innerSolution + sieveInterval;
                    var factorValue = sieve[index];
                    var factorPower = 0;
                    var remQuoRes = this.integerNumber.GetQuotientAndRemainder(factorValue, mappedPrime);
                    while (this.integerNumber.IsAdditiveUnity(remQuoRes.Remainder))
                    {
                        ++factorPower;
                        factorValue = remQuoRes.Quotient;
                        remQuoRes = this.integerNumber.GetQuotientAndRemainder(factorValue, mappedPrime);
                    }

                    sieve[index] = factorValue;
                    matrix[index][currentPrimeIndex] = factorPower;

                    innerSolution += currentPrime;
                }
            }
        }
    }
}
