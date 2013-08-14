using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IRing<T> : IGroup<T>, IMultipliable<T>
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
