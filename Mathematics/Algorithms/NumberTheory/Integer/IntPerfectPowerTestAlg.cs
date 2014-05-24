namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um algoritmo que averigua se um determinado número é uma potência perfeita.
    /// </summary>
    public class IntPerfectPowerTestAlg : IAlgorithm<int, bool>
    {
        /// <summary>
        /// O domínio sobre inteiros.
        /// </summary>
        private IntegerDomain integerDomain = new IntegerDomain();

        /// <summary>
        /// Permite instanciar a classe responsável pela enumeração dos números primos.
        /// </summary>
        private IPrimeNumberIteratorFactory<int> primeNumbersIteratorFactory;

        /// <summary>
        /// Permite criar uma instância da classe responsável pela determinação de potência perfeita.
        /// </summary>
        /// <param name="primeNumbersIteratorFactory">
        /// A fábrica para instanciar o iterador sobre números primos.
        /// </param>
        /// <exception cref="ArgumentNullException">Se a fábrica for nula.</exception>
        public IntPerfectPowerTestAlg(
            IPrimeNumberIteratorFactory<int> primeNumbersIteratorFactory)
        {
            if (primeNumbersIteratorFactory == null)
            {
                throw new ArgumentNullException("primeNumbersIteratorFactory");
            }
            else
            {
                this.primeNumbersIteratorFactory = primeNumbersIteratorFactory;
            }
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
                var maximumTestValue = (int)Math.Floor(Math.Log(innerData) / Math.Log(2));
                var primeNumbersIterator = this.primeNumbersIteratorFactory.CreatePrimeNumberIterator(
                    maximumTestValue + 1);
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
