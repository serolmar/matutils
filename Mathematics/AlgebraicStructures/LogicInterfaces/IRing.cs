using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Define as operações de anel sobre um objecto genérico.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto sobre o qual estão definidas as operações.</typeparam>
    public interface IRing<T> : IGroup<T>, IMultiplication<T>
    {
        /// <summary>
        /// Addiciona um elemento tantas vezes quanto o número especificado.
        /// </summary>
        /// <param name="element">O elemento.</param>
        /// <param name="times">O número de vezes.</param>
        /// <returns>O resultado.</returns>
        T AddRepeated(T element, int times);
    }
}
