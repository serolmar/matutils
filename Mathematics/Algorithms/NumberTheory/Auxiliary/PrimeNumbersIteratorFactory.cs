namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar a instância para um iterador sobre os números primos.
    /// </summary>
    public class PrimeNumbersIteratorFactory : IPrimeNumberIteratorFactory<int>
    {
        public IEnumerable<int> CreatePrimeNumberIterator(int upperLimit)
        {
            return new IntPrimeNumbersIterator(upperLimit);
        }
    }
}
