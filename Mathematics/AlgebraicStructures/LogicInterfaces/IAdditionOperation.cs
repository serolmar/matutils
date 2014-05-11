using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Define a operação de adição aritmética entre dois objectos.
    /// </summary>
    /// <typeparam name="P">O tipo do primeiro objecto a ser adicionado.</typeparam>
    /// <typeparam name="Q">O tipo do segundo objecto a ser adicionado.</typeparam>
    /// <typeparam name="T">O tipo do objecto que constitui o resultado da adição.</typeparam>
    public interface IAdditionOperation<in P, in Q, out T>
    {
        /// <summary>
        /// Aplicar a operação aritmética da adição a dois objectos.
        /// </summary>
        /// <param name="left">O primeiro objecto a ser adicionado.</param>
        /// <param name="right">O segundo objecto a ser adicionado.</param>
        /// <returns>O resultado da adição.</returns>
        T Add(P left, Q right);
    }
}
