namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define a operação de multiplicação aritmética sobre vários tipos de objectos.
    /// </summary>
    /// <typeparam name="P">O tipo do primeiro objecto a ser multiplicado.</typeparam>
    /// <typeparam name="Q">O tipo do segundo objecto a ser multiplicado.</typeparam>
    /// <typeparam name="T">O tipo do objecto resultante da multiplicação.</typeparam>
    public interface IMultiplicationOperation<in P, in Q, out T>
    {
        /// <summary>
        /// Obtém a multiplicação aritmética de dois objectos.
        /// </summary>
        /// <param name="left">O primeiro objecto a ser multiplicado.</param>
        /// <param name="right">O segundo objecto a ser multiplicado.</param>
        /// <returns>O resultado da multiplicação.</returns>
        T Multiply(P left, Q right);
    }
}
