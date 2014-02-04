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
        IEnumerable<NumberType> CreatePrimeNumberIterator(NumberType upperLimit);
    }
}
