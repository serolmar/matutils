namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Cria vectores conceptuais baseados em dicionários.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de valores das entradas do vector.</typeparam>
    public class SparseDictionaryVectorFactory<CoeffType> : IVectorFactory<CoeffType>
    {
        /// <summary>
        /// Cria um vector com o tamanho especificado.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        /// <returns>O vector criado.</returns>
        public IVector<CoeffType> CreateVector(int length)
        {
            return new SparseDictionaryVector<CoeffType>(length);
        }

        /// <summary>
        /// Cria um vector com o tamanho especificado e um valor por defeito.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>O vector criado.</returns>
        public IVector<CoeffType> CreateVector(int length, CoeffType defaultValue)
        {
            return new SparseDictionaryVector<CoeffType>(length, defaultValue);
        }
    }
}
