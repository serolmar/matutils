using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IEuclidenDomain<T> : IRing<T>
    {
        T Quo(T dividend, T divisor);
        T Rem(T dividend, T divisor);

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
