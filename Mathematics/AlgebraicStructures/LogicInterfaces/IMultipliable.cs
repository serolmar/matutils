using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures
{
    public interface IMultipliable<T>
    {
        T MultiplicativeUnity { get; }
        T Multiply(T left, T right);
        bool IsMultiplicativeUnity(T value);
    }
}
