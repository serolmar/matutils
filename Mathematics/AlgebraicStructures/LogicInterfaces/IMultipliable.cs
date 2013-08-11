using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMultipliable<T> : IMultiplicationOperation<T>
    {
        T MultiplicativeUnity { get; }
        bool IsMultiplicativeUnity(T value);
    }
}
