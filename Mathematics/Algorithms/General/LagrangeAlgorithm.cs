using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class LagrangeAlgorithmFor<T, D> : IAlgorithm<T, T, BacheBezoutResult<T>>
        where D : IEuclidenDomain<T>
    {
        public BacheBezoutResult<T> Run(T first, T second)
        {
            throw new NotImplementedException();
        }
    }
}
