using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures
{
    public interface IGroup<T> : IMonoid<T>
    {
        T AdditiveInverse(T number);
    }
}
