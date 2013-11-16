namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define a interface para um construtor de vectores.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo das entradas do vector.</typeparam>
    public interface IVectorFactory<CoeffType>
    {
        /// <summary>
        /// Cria um vector com o tamanho especificado.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        /// <returns>O vector criado.</returns>
        IVector<CoeffType> CreateVector(int length);

        /// <summary>
        /// Cria um vector com o tamanho e o valor por defeito especificados.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>O vector criado.</returns>
        IVector<CoeffType> CreateVector(int length, CoeffType defaultValue);
    }
}
