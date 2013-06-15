using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures
{
    public interface IField<T> : IRing<T>
    {
        T MultiplicativeInverse(T number);
    }
}
