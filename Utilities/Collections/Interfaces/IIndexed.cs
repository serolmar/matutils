namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um objecto indexado.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que representam índices.</typeparam>
    /// <typeparam name="P">O tipo de objectos que representam valores.</typeparam>
    public interface IIndexed<in T, out P>
    {
        /// <summary>
        /// Obtém o objecto especificado pelo índice.
        /// </summary>
        /// <value>
        /// O objecto.
        /// </value>
        /// <param name="index">O índice.</param>
        /// <returns></returns>
        P this[T index] { get; }

        /// <summary>
        /// Obtém o número de elementos.
        /// </summary>
        /// <value>
        /// O número de elementos.
        /// </value>
        int Count { get; }
    }
}
