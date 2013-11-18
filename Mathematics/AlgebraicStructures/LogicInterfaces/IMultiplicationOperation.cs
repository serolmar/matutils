using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMultiplicationOperation<in P, in Q, out T>
    {
        T Multiply(P left, Q right);
    }
}
