namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    public class BigIntegerPrimeNumbersIteratorFactory : IPrimeNumberIteratorFactory<BigInteger>
    {
        public IEnumerable<BigInteger> CreatePrimeNumberIterator(BigInteger upperLimit)
        {
            return new BigIntPrimeNumbsIterator(upperLimit);
        }
    }
}
