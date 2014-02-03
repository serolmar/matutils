using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IEuclidenDomain<T> : IUniqueFactorizationDomain<T>
    {
        /// <summary>
        /// Obtém o quociente entre o dividendo e o divisor.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O quociente.</returns>
        T Quo(T dividend, T divisor);

        /// <summary>
        /// Obtém o resto da divisão do dividendo pelo divisor.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O resto da divisão.</returns>
        T Rem(T dividend, T divisor);

        /// <summary>
        /// Obtém o quociente e o resto da divisão do dividendo pelo divisor.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O quociente e o resto.</returns>
        DomainResult<T> GetQuotientAndRemainder(T dividend, T divisor);

        /// <summary>
        /// Obtém o grau do valor no domínimo euclideano.
        /// </summary>
        /// <remarks>
        /// Admite-se que é possível escrever D=qd+f onde grau(f) menor que grau(d) e q
        /// é um número arbitrário.
        /// </remarks>
        /// <param name="value">O valor do qual se pretende determinar o grau.</param>
        /// <returns>O grau.</returns>
        uint Degree(T value);
    }
}
