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
    public class QuadraticFieldSieve : IAlgorithm<int, int, int, Tuple<int,int>>
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

                var resSolAlg = new ResSolAlgorithm();
                var sqrt = (int)Math.Floor(Math.Sqrt(innerData));
                var innerSieveInterval = sieveInterval;
                if (innerSieveInterval > innerData - sqrt - 1)
                {
                    innerSieveInterval = innerData - sqrt - 1;
                }

                var quadraticSieve = new int[innerSieveInterval];
                for (int i = 0; i < quadraticSieve.Length; ++i)
                {
                    quadraticSieve[i] = (sqrt + i + 1) ^ 2 - innerData;
                }
            }

            throw new NotImplementedException();
        }
    }
}
