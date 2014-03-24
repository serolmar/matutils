namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IMultiplication<T> : IMultiplicationOperation<T, T, T>
    {
        T MultiplicativeUnity { get; }
        bool IsMultiplicativeUnity(T value);
    }
}
