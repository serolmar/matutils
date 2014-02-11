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
    public class QuadraticFieldSieve : IAlgorithm<int, int, int, Tuple<int, int>>
    {
        /// <summary>
        /// Algoritmo que permite resolver o sistema de equações.
        /// </summary>
        private IAlgorithm<IMatrix<int>, IMatrix<int>, LinearSystemSolution<int>>
            linearSystemAlgorithm;

        /// <summary>
        /// Permite criar instâncias do corpo modular.
        /// </summary>
        private IModularFieldFactory<int> modularFieldFactory;

        /// <summary>
        /// Permite criar instâncias de iteradores para números primos positivos.
        /// </summary>
        IPrimeNumberIteratorFactory<int> primesIteratorFactory;

        /// <summary>
        /// Permite determinar a parte inteira da raiz quadrada.
        /// </summary>
        IAlgorithm<int, int> integerSquareRootAlgorithm;

        /// <summary>
        /// Permite efectuar as operações sobre os números inteiros.
        /// </summary>
        IIntegerNumber<int> integerNumber;

        /// <summary>
        /// O corpo responsável pelas operações módulo dois.
        /// </summary>
        private ModularIntegerField field;

        public QuadraticFieldSieve()
        {
            this.integerNumber = new IntegerDomain();
            this.modularFieldFactory = new ModularIntegerFieldFactory();
            this.field = new ModularIntegerField(this.integerNumber.MapFrom(2));
            this.linearSystemAlgorithm = new DenseCondensationLinSysAlgorithm<int>(
                this.field);
            this.primesIteratorFactory = new PrimeNumbersIteratorFactory();
            this.integerSquareRootAlgorithm = new IntegerSquareRootAlgorithm();
        }

        /// <summary>
        /// Obtém a factorização do módulo do número especificado.
        /// </summary>
        /// <param name="data">O número.</param>
        /// <param name="factorBase">O limite máximo para os números primos da base.</param>
        /// <param name="sieveInterval">O intervalo sobre os quais são crivados os números.</param>
        /// <returns>A decomposição do número especificado num produto de dois factores.</returns>
        public Tuple<int, int> Run(int data, int factorBase, int sieveInterval)
        {
            var innerData = this.integerNumber.GetNorm(data);
            var two = this.integerNumber.MapFrom(2);
            if (this.integerNumber.Compare(factorBase, 2) < 0)
            {
                throw new ArgumentException("Factor base limit can't be less than two.");
            }
            else if (this.integerNumber.Compare(sieveInterval, this.integerNumber.MultiplicativeUnity) < 0)
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
                var legendreAlgorithm = new LegendreJacobiSymbolAlgorithm<int>(this.integerNumber);
                foreach (var prime in primesIterator)
                {
                    if (legendreAlgorithm.Run(innerData, prime) == 1)
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
                        new ZeroMatrix<int, ModularIntegerField>(
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
        /// <param name="innerDataModularField">O corpo responsável pelas operações de multiplicação.</param>
        /// <param name="primesList">A lista dos números primos da base.</param>
        /// <param name="innerData">O número que está a ser factorizado.</param>
        /// <returns>Os factores.</returns>
        private Tuple<int, int> GetSolution(
            LinearSystemSolution<int> solution,
            List<int[]> matrixList,
            List<int> primesList,
            int innerData)
        {
            var innerDataModularField = this.modularFieldFactory.CreateInstance(innerData);
            var countFactors = primesList.Count - 1;
            foreach (var solutionBase in solution.VectorSpaceBasis)
            {
                var firstValue = 1;
                var factorsCount = new Dictionary<int, int>();
                for (int i = 0; i < matrixList.Count; ++i)
                {
                    var currentMatrixLine = matrixList[i];
                    if (solutionBase[i] == 1)
                    {
                        firstValue = innerDataModularField.Multiply(
                            firstValue,
                            this.integerNumber.GetNorm(currentMatrixLine[currentMatrixLine.Length - 1]));

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

                var secondValue = 1;
                foreach (var factorCountKvp in factorsCount)
                {
                    var primePower = MathFunctions.Power(
                        factorCountKvp.Key,
                        factorCountKvp.Value / 2,
                        innerDataModularField);

                    secondValue = innerDataModularField.Multiply(
                        secondValue,
                        primePower);
                }

                if (firstValue != secondValue)
                {
                    var firstFactor = MathFunctions.GreatCommonDivisor(
                        innerData,
                        Math.Abs(firstValue - secondValue),
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
            int innerData,
            int sqrt,
            int sieveInterval,
            List<int> primesList)
        {
            var innerSieveInterval = sieveInterval;
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

            var resSolAlg = new ResSolAlgorithm<int>(
                this.modularFieldFactory, 
                this.integerNumber);
            for (int i = 1; i < primesList.Count; ++i)
            {
                var currentPrime = primesList[i];
                var quadraticRes = resSolAlg.Run(innerData, currentPrime);
                var firstRes = (quadraticRes[0] - sqrt) % currentPrime;
                var secondRes = (quadraticRes[1] - sqrt) % currentPrime;
                if ((firstRes - secondRes) % currentPrime == 0)
                {
                    // Apenas uma solução encontrada
                    this.SetupQuadResSolution(
                        firstRes,
                        currentPrime,
                        i,
                        sieveInterval,
                        sieveValues,
                        matrixList);
                }
                else
                {
                    // Duas soluções encontradas
                    this.SetupQuadResSolution(
                        firstRes,
                        currentPrime,
                        i,
                        sieveInterval,
                        sieveValues,
                        matrixList);

                    this.SetupQuadResSolution(
                        secondRes,
                        currentPrime,
                        i,
                        sieveInterval,
                        sieveValues,
                        matrixList);
                }
            }

            // Tratamento do número primo 2
            var two = this.integerNumber.MapFrom(2);
            var eight = this.integerNumber.MapFrom(8);
            if (this.integerNumber.IsMultiplicativeUnity(this.integerNumber.Rem(innerData,eight)))
            {
                var index = 0;
                while (index < sieveValues.Length && 
                    sieveValues[index] % 2 != 0)
                {
                    ++index;
                }

                while (index < sieveValues.Length)
                {
                    var factorValue = sieveValues[index];
                    var factorPower = 0;
                    while (factorValue % 2 == 0)
                    {
                        ++factorPower;
                        factorValue = factorValue / 2;
                    }

                    sieveValues[index] = factorValue;
                    matrixList[index][0] = factorPower;
                    index += 2;
                }
            }
            else
            {
                var index = 0;
                while (index < sieveValues.Length && sieveValues[index] % 2 != 0)
                {
                    ++index;
                }

                while (index < sieveValues.Length)
                {
                    var factorValue = sieveValues[index] / 2;
                    sieveValues[index] = factorValue;
                    matrixList[index][0] = 1;
                    index += 2;
                }
            }

            // Tratamento do sinal
            primesList.Add(-1);
            for (int i = 0; i < matrixList.Count; ++i)
            {
                if (sieveValues[i] < 0)
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
                if (currentSieveValue == 1 || currentSieveValue == -1)
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
        private int[] ComputeSieveValues(int innerData, int sieveInterval, int sqrt)
        {
            var length = 2 * sieveInterval + 1;
            var result = new int[2 * sieveInterval + 1];
            result[sieveInterval] = sqrt;
            for (int i = 1; i <= sieveInterval; ++i)
            {
                result[sieveInterval - i] = (sqrt - i) * (sqrt - i) - innerData;
                result[sieveInterval + i] = (sqrt + i) * (sqrt + i) - innerData;
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
            int[] sieve,
            List<int[]> matrix)
        {
            var innerSolution = solution - currentPrime;
            if (solution < sieveInterval)
            {
                while (innerSolution > -sieveInterval)
                {
                    var index = innerSolution + sieveInterval;
                    var factorValue = sieve[index];
                    var factorPower = 0;
                    while (factorValue % currentPrime == 0)
                    {
                        ++factorPower;
                        factorValue = factorValue / currentPrime;
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
                    while (factorValue % currentPrime == 0)
                    {
                        ++factorPower;
                        factorValue = factorValue / currentPrime;
                    }

                    sieve[index] = factorValue;
                    matrix[index][currentPrimeIndex] = factorPower;

                    innerSolution += currentPrime;
                }
            }
        }
    }
}
