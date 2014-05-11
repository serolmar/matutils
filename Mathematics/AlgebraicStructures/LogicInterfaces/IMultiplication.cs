namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma operação de multiplicação aritmética sobre um mesmo tipo de objecto.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto envolvido na multiplicação.</typeparam>
    public interface IMultiplication<T> : IMultiplicationOperation<T, T, T>
    {
        /// <summary>
        /// Permite obter a unidade multiplicativa.
        /// </summary>
        T MultiplicativeUnity { get; }

        /// <summary>
        /// Permite averiguar se um objecto constitui uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">O objecto a ser analisado.</param>
        /// <returns>Verdadeiro caso o objecto seja uma unidade multiplicativa e falso caso contrário.</returns>
        bool IsMultiplicativeUnity(T value);
    }
}
