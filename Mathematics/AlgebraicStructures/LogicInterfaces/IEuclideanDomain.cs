using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures
{
    public interface IEuclidenDomain<T> : IRing<T>
    {
        T Quo(T dividend, T divisor);
        T Rem(T dividend, T divisor);
    }
}
