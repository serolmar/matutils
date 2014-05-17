namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Cria enumeradores para números primos representados por inteiros de precisão arbitrária.
    /// </summary>
    public class BigIntegerPrimeNumbersIteratorFactory : IPrimeNumberIteratorFactory<BigInteger>
    {
        /// <summary>
        /// Cria um enumerador para números primos.
        /// </summary>
        /// <param name="upperLimit">O limite superior do enumerador.</param>
        /// <returns>O enumerador.</returns>
        public IEnumerable<BigInteger> CreatePrimeNumberIterator(BigInteger upperLimit)
        {
            return new BigIntPrimeNumbsIterator(upperLimit, new BigIntSquareRootAlgorithm());
        }
    }
}
