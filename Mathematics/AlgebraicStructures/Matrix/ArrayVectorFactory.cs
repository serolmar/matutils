namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Cria vectores conceptuais baseados em vectores de memória.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos valores das entradas do vector.</typeparam>
    public class ArrayVectorFactory<CoeffType> : IMathVectorFactory<CoeffType>
    {
        /// <summary>
        /// Cria um vector com o tamanho especificado.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        /// <returns>O vector criado.</returns>
        public IMathVector<CoeffType> CreateVector(int length)
        {
            return new ArrayMathVector<CoeffType>(length);
        }

        /// <summary>
        /// Cria um vector com o tamanho especificado e um valor por defeito.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>O vector criado.</returns>
        public IMathVector<CoeffType> CreateVector(int length, CoeffType defaultValue)
        {
            return new ArrayMathVector<CoeffType>(length, defaultValue);
        }
    }
}
