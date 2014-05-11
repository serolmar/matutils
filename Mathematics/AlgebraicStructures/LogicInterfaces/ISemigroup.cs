using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Define as operações de semi-grupo sobre um objecto genérico.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos sobre os quais estão defindas as operações de semi-grupo.</typeparam>
    public interface ISemigroup<T> : IEqualityComparer<T>, IAdditionOperation<T, T, T>
    {
    }
}
