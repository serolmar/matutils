using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures
{
    public interface IRing<T> : IGroup<T>, IMultipliable<T>
    {
    }
}
