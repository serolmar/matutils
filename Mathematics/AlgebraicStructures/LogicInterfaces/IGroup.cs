using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Define as operações de grupo sobre um objecto.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto sobre o qual são efectuadas as operações de grupo.</typeparam>
    public interface IGroup<T> : IMonoid<T>
    {
        /// <summary>
        /// Obtém a inversa aditiva de um objecto.
        /// </summary>
        /// <param name="number">O objecto do qual ser pretende a inversa aditiva.</param>
        /// <returns>A inversa aditiva.</returns>
        T AdditiveInverse(T number);
    }
}
