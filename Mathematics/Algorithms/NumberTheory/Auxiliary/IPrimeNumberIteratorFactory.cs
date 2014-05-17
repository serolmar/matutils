namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Fábrica que permite criar iteradores para números primos.
    /// </summary>
    /// <typeparam name="NumberType">O tipo de número.</typeparam>
    public interface IPrimeNumberIteratorFactory<NumberType>
    {
        /// <summary>
        /// Cria um enumerador para números primos.
        /// </summary>
        /// <param name="upperLimit">O limite superior.</param>
        /// <returns>O enumerador para números primos.</returns>
        IEnumerable<NumberType> CreatePrimeNumberIterator(NumberType upperLimit);
    }
}
