namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar a instância para um iterador sobre os números primos representados por inteiros.
    /// </summary>
    public class PrimeNumbersIteratorFactory : IPrimeNumberIteratorFactory<int>
    {
        /// <summary>
        /// Cria um enumerador para números primos representados por inteiros.
        /// </summary>
        /// <param name="upperLimit">O limite superior para o enumerador.</param>
        /// <returns>O enumerador.</returns>
        public IEnumerable<int> CreatePrimeNumberIterator(int upperLimit)
        {
            return new IntPrimeNumbersIterator(upperLimit, new IntegerSquareRootAlgorithm());
        }
    }
}
