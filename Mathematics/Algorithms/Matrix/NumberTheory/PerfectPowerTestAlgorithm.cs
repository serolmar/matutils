namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um algoritmo que averigua se um determinado número é uma potência perfeita.
    /// </summary>
    public class PerfectPowerTestAlgorithm : IAlgorithm<int, bool>
    {
        /// <summary>
        /// O domínio sobre inteiros.
        /// </summary>
        private IntegerDomain integerDomain = new IntegerDomain();

        public PerfectPowerTestAlgorithm()
        {
        }

        /// <summary>
        /// Verifica se o módulo do número especificado consiste numa potência perfeita.
        /// </summary>
        /// <param name="data">O número a testar.</param>
        /// <returns>Verdadeiro caso o número seja uma potência perfeita e falso no caso contrário.</returns>
        public bool Run(int data)
        {
            var innerData = Math.Abs(data);

            if (innerData == 0)
            {
                return true;
            }
            else if (innerData == 1)
            {
                return true;
            }
            else
            {
                var maximumTestValue = (int)Math.Floor(Math.Log(innerData)/Math.Log(2));
                var primeNumbersIterator = new PrimeNumbersIterator(maximumTestValue + 1);
                foreach (var prime in primeNumbersIterator)
                {
                    var root = (int)Math.Floor(Math.Pow(innerData, 1.0 / prime));
                    var power = MathFunctions.Power(root, prime, this.integerDomain);
                    if (power == innerData)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
