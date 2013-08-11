using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMultiplicationOperation<T>
    {
        T Multiply(T left, T right);
    }
}
