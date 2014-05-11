using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Define as operações de monóide sobre objectos genéricos.
    /// </summary>
    /// <typeparam name="T">O tipo do objecto sobre o qual se pretendem efectuar operações de monóide.</typeparam>
    public interface IMonoid<T> : ISemigroup<T>
    {
        /// <summary>
        /// Obtém a unidade aditiva associada ao monóide.
        /// </summary>
        T AdditiveUnity { get; }

        /// <summary>
        /// Permite averiguar se um determinado objecto é uma unidade aditiva.
        /// </summary>
        /// <param name="value">O objecto a ser avaliado.</param>
        /// <returns>Verdadeiro caso o objecto seja uma unidade aditiva e falso caso contrário.</returns>
        bool IsAdditiveUnity(T value);
    }
}
