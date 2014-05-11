using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Define as operações de corpo sobre objectos genéricos.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto sobre o qual são realizadas as operações de corpo.</typeparam>
    public interface IField<T> : IRing<T>
    {
        /// <summary>
        /// Obtém a inversa multiplicativa de um objecto.
        /// </summary>
        /// <param name="number">O objecto do qual se pretende obter a inversa multiplicativa.</param>
        /// <returns>A inversa multiplicativa.</returns>
        T MultiplicativeInverse(T number);
    }
}
