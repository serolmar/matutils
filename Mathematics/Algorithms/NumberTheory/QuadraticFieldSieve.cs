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
        /// Obtém a factorização do módulo do número especificado.
        /// </summary>
        /// <param name="data">O número.</param>
        /// <returns>A decomposição do número especificado num produto de dois factores.</returns>
        public Tuple<int, int> Run(int data, int factorBase, int sieveInterval)
        {
            var innerData = Math.Abs(data);
            if (factorBase < 2)
            {
                throw new ArgumentException("Factor base limit can't be less than two.");
            }
            else if (sieveInterval < 1)
            {
                throw new ArgumentException("Sieve interval can't be less than one.");
            }
            if (innerData == 0)
            {
                throw new MathematicsException("Zero has no factors.");
            }
            else if (innerData == 1 || innerData == 2 || innerData == 3)
            {
                return Tuple.Create(innerData, 1);
            }
            else
            {
                var primesList = new List<int>();
                var primesIterator = new PrimeNumbersIterator(factorBase);
                var legendreAlgorithm = new LegendreJacobiSymbolAlgorithm();
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

                var sieveMatrix = this.ComputeSieveStep(innerData, sieveInterval, primesList);

                // Implementação do algoritmo associado à combinação linear para a obtenção do resultado
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Determina a matriz que resulta da aplicação do crivo quadrático.
        /// </summary>
        /// <param name="innerData">O valor a ser factorizado.</param>
        /// <param name="sieveInterval">O intervalo de crivo.</param>
        /// <param name="primesList">A base de primos.</param>
        /// <returns>A matriz com o vector.</returns>
        public Tuple<ArrayBitMatrix, int[]> ComputeSieveStep(int innerData, int sieveInterval, List<int> primesList)
        {
            var sqrt = (int)Math.Floor(Math.Sqrt(innerData));
            var innerSieveInterval = sieveInterval;
            if (innerSieveInterval > innerData - sqrt - 1)
            {
                innerSieveInterval = innerData - sqrt - 1;
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

            var resSolAlg = new ResSolAlgorithm();
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
            if (innerData % 8 == 1)
            {
                var index = 0;
                while (index < sieveValues.Length && sieveValues[index] % 2 != 0)
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

            var matrix = new ArrayBitMatrix(matrixList.Count, primesList.Count, 0);
            for (int i = 0; i < matrixList.Count; ++i)
            {
                for (int j = 0; j < primesList.Count; ++j)
                {
                    matrix[i, j] = matrixList[i][j];
                }
            }

            var vector = new int[matrixList.Count];
            for (int i = 0; i < matrixList.Count; ++i)
            {
                vector[i] = matrixList[i][primesList.Count];
            }

            return Tuple.Create(matrix, vector);
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
