namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um algoritmo que permite determinar o valor da função phi de Euler.
    /// </summary>
    public class EulerTotFuncAlg : IAlgorithm<int, int>
    {
        /// <summary>
        /// Determina a quantidade de números primos inferiores ou iguais ao módulo do valor especificado.
        /// </summary>
        /// <param name="data">O valor a ser analisado.</param>
        /// <returns>A quantidade de números primos nestas condições.</returns>
        public int Run(int data)
        {
            var innerData = Math.Abs(data);
            if (data == 0 || data == 1)
            {
                return 0;
            }
            else
            {
                var result = 1;
                var sqrt = (int)Math.Floor(Math.Sqrt(innerData));
                var primeNumbersIterator = new PrimeNumbersIterator(sqrt);
                foreach (var prime in primeNumbersIterator)
                {
                    if (innerData % prime == 0)
                    {
                        result *= (prime - 1);
                        innerData = innerData / prime;
                        while (innerData % prime == 0)
                        {
                            result *= prime;
                            innerData = innerData / prime;
                        }

                        if (innerData == 1)
                        {
                            return result;
                        }
                    }
                }

                // Neste ponto verifica-se que "innerDta" é um número primo.
                return result * (innerData - 1);
            }
        }
    }
}
